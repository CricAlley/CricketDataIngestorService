using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using AutoMapper;

using ElasticRepository;

using PlayerDataGenerator.Data;
using PlayerDataGenerator.YamlParser;

using Elastic = ElasticRepository.Entities;

namespace PlayerDataGenerator
{
    internal interface ICricketDataIngestor
    {
        void ExtractPlayers();
        void IngestMatchData();
    }

    internal class CricketDataIngestor : ICricketDataIngestor
    {
        private readonly GeneralSettings _generalSettings;
        private readonly CricketContext _playerContext;
        private readonly IPlayerScriptGenerator _playerScriptGenerator;
        private readonly IMapper _mapper;
        private readonly Dictionary<string, Player> _players;
        private readonly Dictionary<string, string> _failedPlayers = new Dictionary<string, string>();
        private List<ExcludedTeam> _excludedTeams;
        private List<Player> _dbPlayers;
        private List<PlayerAliasMapping> _playerAliases;
        private List<string> _teams = new List<string>();

        public CricketDataIngestor(GeneralSettings generalSettings, CricketContext playerContext, IPlayerScriptGenerator playerScriptGenerator, IMapper mapper)
        {
            _generalSettings = generalSettings;
            _playerContext = playerContext;
            _playerScriptGenerator = playerScriptGenerator;
            _mapper = mapper;
            _players = new Dictionary<string, Player>();

            _excludedTeams = playerContext.ExcludedTeams.ToList();
            _dbPlayers = playerContext.Players.ToList();
            _playerAliases = playerContext.PlayerAliasMapping.ToList();

            //bbl_male
            //ipl_male
            //ntb_male
            //odis_male
            //t20s_male
            // cpl_male
        }

        public void ExtractPlayers()
        {
            Console.WriteLine("Player Data Extraction started");
            var directoryInfo = new DirectoryInfo(_generalSettings.InputFolderPath);

            var directoryInfos = directoryInfo.GetDirectories();
            var yamlParser = new YamlParser.YamlParser();
            var stringBuilder = new StringBuilder();
            var teamsBuilder = new StringBuilder();

            int count = 0;

            foreach (var directory in directoryInfos)
            {
                var files = directory.GetFiles("*.yaml");

                foreach (var file in files)
                {
                    var match = yamlParser.Parse(file.FullName);

                    var isBothTeamsExcluded = match.MatchInfo.Teams.All(s => _excludedTeams.Any(t => t.TeamName.Equals(s, StringComparison.InvariantCultureIgnoreCase)));

                    if (isBothTeamsExcluded)
                    {
                        continue;
                    }

                    foreach (var team in match.MatchInfo.Teams)
                    {
                        if (_teams.All(t => t != team))
                        {
                            teamsBuilder.AppendLine(@"_includedTeams.Add(" + $"{team}" + ");");
                            _teams.Add(team);
                        }
                    }


                    var players = match.GetPlayers();

                    foreach (var player in players)
                    {
                        try
                        {
                            UpdatePlayer(player);
                        }
                        catch (Exception e)
                        {
                            if (_failedPlayers.ContainsKey(player))
                            {
                                continue;
                            }

                            count++;

                            if (_players.ContainsKey(player))
                            {
                                _failedPlayers.Add(player, _players[player].FullName);
                            }
                            else
                            {
                                _failedPlayers.Add(player, player);
                            }

                            stringBuilder.AppendLine(
                                $"exception: {e.Message}, Player Name: {player} + yaml- {file.Name}, search text - {match.MatchInfo.Dates.First().ToString("dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture)} {string.Join(" vs ", match.MatchInfo.Teams)} ");
                        }

                    }
                }
            }

            stringBuilder.Append($"Unable to Map {count} Players ");

            var failedPlayerpath = $"{_generalSettings.OutputFolderPath}\\{Constants.FailedPlayerFile}";
            var includedTeamsFilePath = $"{_generalSettings.OutputFolderPath}\\{Constants.IncludedTeams}";
            WriteToFile(failedPlayerpath, stringBuilder.ToString());
            WriteToFile(includedTeamsFilePath, teamsBuilder.ToString());           
            Console.WriteLine("Extraction Complete");

            if (_failedPlayers.Count > 0)
            {
                throw new Exception($"there are {count} unmapped players. Please Map them.");
            }
            else
            {
                _playerScriptGenerator.GenerateScript();
            }
        }

        private static void WriteToFile(string failedPlayerpath, string content)
        {
            FileInfo file = new FileInfo(failedPlayerpath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            File.WriteAllText(file.FullName, content);
        }

        private void UpdatePlayer(string player)
        {
            player = player.Replace(" (sub)", "");

            if (_players.ContainsKey(player) || _failedPlayers.ContainsKey(player) || _playerAliases.Any(pa => pa.CricsheetName.Equals(player, StringComparison.InvariantCultureIgnoreCase))) return;
            
            Player p = _dbPlayers.SingleOrDefault(p => p.CricsheetName != null && p.CricsheetName.Equals(player));

            if (p != null) return;            

            List<Player> foundPlayers = new List<Player>();

            var names = player.Split(' ');

            var lastName = names.Last();
            var firstName = names.First();
            var isInitials = firstName.ToCharArray().All(c => c == char.ToUpper(c));

            var middleName = string.Empty;
            if (names.Length > 2) middleName = names.Skip(1).First();

            if (isInitials)
            {
                var dbPlayers = _dbPlayers.Where(player1 =>
                    player1.FullName.ToUpper().Contains(lastName.ToUpper()) && player1.IsActive).ToList();

                if (!dbPlayers.Any())
                {
                    throw new Exception("Player not present");

                }

                foreach (var dbPlayer in dbPlayers)
                {
                    var dbNames = dbPlayer.FullName.Split(' ');
                    var dbLastName = dbNames.Last();
                    var dbFirstName = dbNames.First();
                    var isDbNameInitials = dbFirstName.ToCharArray().All(c => c == char.ToUpper(c));

                    var isfound = false;
                    if (dbLastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isfound = true;
                        var j = 0;
                        for (var i = 0; i < firstName.Length; i++, j++)
                        {
                            if (dbNames[i][0] != firstName[i])
                            {
                                isfound = false;
                                break;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(middleName) && dbNames[j].ToUpper() != middleName.ToUpper())
                        {
                            isfound = false;
                        }

                        if (isDbNameInitials)
                        {
                            isfound = dbFirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase);
                        }
                    }

                    if (isfound)
                    {
                        foundPlayers.Add(dbPlayer);
                    }
                }

                if (!foundPlayers.Any())
                {
                    throw new NullReferenceException("Player not found");
                }
                else if (foundPlayers.Count > 1)
                {
                    throw new AmbiguousMatchException("Multiple Players found for same key");
                }
                else
                {
                    var dbPlayer = foundPlayers.First();
                    _players.Add(player, dbPlayer);
                    dbPlayer.CricsheetName = player;
                }
            }
            else
            {
                var dbPlayers = _dbPlayers.Where(player1 =>
                    player1.FullName.ToUpper().Contains(firstName.ToUpper()) && player1.IsActive).ToList();
                if (!dbPlayers.Any())
                {
                    throw new Exception("Player not present");
                }

                foreach (var dbPlayer in dbPlayers)
                {
                    var dbNames = dbPlayer.Name.Split(' ');
                    var dbFirstName = dbNames.First();
                    var dbLastName = dbNames.Last();
                    var dbMiddleName = string.Empty;
                    if (dbNames.Length > 2) dbMiddleName = dbNames.Skip(1).FirstOrDefault();

                    if (string.Equals(firstName.ToUpper(), dbFirstName.ToUpper(), StringComparison.Ordinal) &&
                        string.Equals(lastName, dbLastName, StringComparison.CurrentCultureIgnoreCase) &&

                        ((middleName == null && dbMiddleName ==null) ||
                        middleName.Equals(dbMiddleName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        foundPlayers.Add(dbPlayer);
                    }
                }

                if (!foundPlayers.Any())
                {
                    throw new NullReferenceException("Player not found");
                }
                else if (foundPlayers.Count > 1)
                {
                    throw new AmbiguousMatchException("Multiple Players found for same key");
                }
                else
                {
                    var dbPlayer = foundPlayers.First();
                    _players.Add(player, dbPlayer);
                    dbPlayer.CricsheetName = player;
                }
            }

            _playerContext.SaveChanges();
        }

        public void IngestMatchData()
        {
            Console.WriteLine("Match data ingestion started");
            var directoryInfo = new DirectoryInfo(_generalSettings.InputFolderPath);

            var directoryInfos = directoryInfo.GetDirectories();
            var yamlParser = new YamlParser.YamlParser();
            var elasticClient = new ElasticClientProvider().GetElasticClient(_generalSettings.ElasticUrl);
            string indexName;


            foreach (var directory in directoryInfos)
            {
                var files = directory.GetFiles("*.yaml");


                indexName = directory.Name;
                elasticClient.Indices.Delete(indexName);
                Console.WriteLine($"Creating index : {indexName}");


                foreach (var file in files)
                {
                    var match = yamlParser.Parse(file.FullName);

                    var isBothTeamsExcluded = match.MatchInfo.Teams.All(s => _excludedTeams.Any(t => t.TeamName.Equals(s, StringComparison.InvariantCultureIgnoreCase)));

                    if (isBothTeamsExcluded)
                    {
                        continue;
                    }

                    var balls = new List<Elastic.Ball>();

                    var matchId = GetMatchId(match.MatchInfo, file);

                    foreach (var inning in match.Innings)
                    {
                        var inningValue = inning.Values.First();
                        var battingTeam = inningValue.Team;
                        var bowlingTeam = match.MatchInfo.Teams.First(t => !t.Equals(inningValue.Team));

                        foreach (var delivery in inningValue.Deliveries)
                        {
                            var deliveryValue = delivery.Values.First();
                            var ball = _mapper.Map<Elastic.Ball>(deliveryValue);
                            var bowler = GetPlayer(deliveryValue.Bowler);
                            var elasticBowler = _mapper.Map<Elastic.Player>(bowler);
                            elasticBowler.Team = bowlingTeam;
                            ball.Bowler = elasticBowler;

                            var batsman = GetPlayer(deliveryValue.Batsman); ;
                            var elasticBatsman = _mapper.Map<Elastic.Player>(batsman);
                            elasticBatsman.Team = battingTeam;
                            ball.Batsman = elasticBatsman;

                            var nonStriker = GetPlayer(deliveryValue.NonStriker); ;
                            var elasticNonStriker = _mapper.Map<Elastic.Player>(nonStriker);
                            elasticNonStriker.Team = battingTeam;
                            ball.NonStriker = elasticBatsman;

                            ball.DeliveryKey = delivery.Keys.First();

                            var innings = _mapper.Map<Elastic.Inning>(inningValue);
                            var matchInfo = match.MatchInfo;

                            innings.Innings = inning.First().Key;
                            innings.BowlingTeam = matchInfo.Teams.First(s => s != innings.BattingTeam);
                            ball.Inning = innings;

                            var ballMatch = _mapper.Map<Elastic.Match>(matchInfo);
                            ball.Match = ballMatch;
                            ball.Match.MatchId = matchId;
                            var inningsId = matchId + innings.Innings;
                            ball.Inning.InningsId = inningsId;
                            ball.OverId = inningsId + ball.Over;

                            balls.Add(ball);
                        }
                    }

                    var bulkIndexResponse = elasticClient.Bulk(b => b.Index(indexName)
                                                                .IndexMany(balls));
                }
            }

            Console.WriteLine("Elastic Ingestion complete");
        }

        private Player GetPlayer(string playerName)
        {
            
            try
            {
                Player player = _dbPlayers.SingleOrDefault(p => p.CricsheetName != null && p.CricsheetName.Equals(playerName, StringComparison.InvariantCultureIgnoreCase));
                if (player == null)
                {
                    var playerAliasMapping
                        = _playerAliases.FirstOrDefault(p => p.CricsheetName.Equals(playerName, StringComparison.InvariantCultureIgnoreCase));

                    return _dbPlayers.Single(p => p.CricInfoId == playerAliasMapping.CricInfoId);
                }

                return player;
            }
            catch(Exception e)
            {
                Console.WriteLine($"PlayerName failed : {playerName}. Exception: {e.Message}");
                throw;
            }
        }

        private static string GetMatchId(MatchInfo matchInfo, FileInfo file)
        {
            return matchInfo.Dates.First().ToLongDateString() + matchInfo.City +
                   string.Join("", matchInfo.Teams.Select(s => s.Replace(" ", ""))) +
                   file.Name;
        }

    }
}
