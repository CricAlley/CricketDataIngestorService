using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using AutoMapper;

using ElasticRepository;

using PlayerDataGenerator.Data;
using PlayerDataGenerator.JsonParser;

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
        private readonly IEmailSender _emailSender;
        private readonly Dictionary<string, Player> _players;
        private readonly Dictionary<string, string> _failedPlayers = new Dictionary<string, string>();
        private List<ExcludedTeam> _excludedTeams;
        private List<Player> _dbPlayers;
        private List<PlayerAliasMapping> _playerAliases;
        private List<string> _teams = new List<string>();

        public CricketDataIngestor(GeneralSettings generalSettings, CricketContext playerContext, IPlayerScriptGenerator playerScriptGenerator, IMapper mapper, IEmailSender emailSender)
        {
            _generalSettings = generalSettings;
            _playerContext = playerContext;
            _playerScriptGenerator = playerScriptGenerator;
            _mapper = mapper;
            _emailSender = emailSender;
            _players = new Dictionary<string, Player>();

            _excludedTeams = playerContext.ExcludedTeams.ToList();
            _dbPlayers = playerContext.Players.ToList();
            _playerAliases = playerContext.PlayerAliasMapping.ToList();            
        }

        public void ExtractPlayers()
        {
            Console.WriteLine("Player Data Extraction started");
            var directoryInfo = new DirectoryInfo(_generalSettings.InputFolderPath);

            var directoryInfos = directoryInfo.GetDirectories();
            var yamlParser = new YamlParser.YamlParser();
            var failedPlayerSB = new StringBuilder();
            var teamsBuilder = new StringBuilder();

            int count = 0;

            foreach (var directory in directoryInfos)
            {
                var files = directory.GetFiles("*.json");

                foreach (var file in files)
                {
                    var jsonParser = new JsonParser.JsonParser();
                    var match = jsonParser.Parse(file.FullName);

                    var isBothTeamsExcluded = match.Info.Teams.All(s => _excludedTeams.Any(t => t.TeamName.Equals(s, StringComparison.InvariantCultureIgnoreCase)));

                    if (isBothTeamsExcluded)
                    {
                        continue;
                    }

                    foreach (var team in match.Info.Teams)
                    {
                        if (_teams.All(t => t != team))
                        {
                            teamsBuilder.AppendLine(@"_includedTeams.Add(" + $"{team}" + ");");
                            _teams.Add(team);
                        }
                    }

                    foreach (var player in match.Players)
                    {
                        var playerName = player.Value.Name;
                        try
                        {
                            UpdatePlayer(player.Value);
                        }
                        catch (Exception e)
                        {
                            if (_failedPlayers.ContainsKey(playerName))
                            {
                                continue;
                            }

                            count++;

                            if (_players.ContainsKey(playerName))
                            {
                                _failedPlayers.Add(playerName, _players[playerName].FullName);
                            }
                            else
                            {
                                _failedPlayers.Add(playerName, playerName);
                            }

                            failedPlayerSB.AppendLine(
                                $"{e.Message},{playerName}, {file.Name},{player.Value.Identifier} ,search text - {match.Info.Dates.First()} {string.Join(" vs ", match.Info.Teams)} ");
                        }

                    }
                }
            }

            failedPlayerSB.Append($"Unable to Map {count} Players ");

            var failedPlayerpath = $"{_generalSettings.OutputFolderPath}/{Constants.FailedPlayerFile}";
            var includedTeamsFilePath = $"{_generalSettings.OutputFolderPath}/{Constants.IncludedTeams}";

            var fplayer = failedPlayerSB.ToString();
            var includedTeams = teamsBuilder.ToString();
            WriteToFile(failedPlayerpath, fplayer);
            WriteToFile(includedTeamsFilePath, includedTeams);

            if (_emailSender.IsMailSettingsConfigured())
            {
                _emailSender.Email(fplayer, Constants.FailedPlayerFile);
                _emailSender.Email(includedTeams, Constants.IncludedTeams);
            }
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

        private void UpdatePlayer(JsonParser.MatchPlayer value)
        {
            var identifiedPlayer = _dbPlayers.SingleOrDefault(p => p.Identifier != null && p.Identifier.Equals(value.Identifier));

            if(identifiedPlayer != null)
            {
                if (identifiedPlayer.CricsheetName == null || !identifiedPlayer.CricsheetName.Equals(value.Name))
                {
                    identifiedPlayer.CricsheetName = value.Name;
                    _playerContext.SaveChanges();
                }

                return;
            }

            var player = value.Name;

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
                    dbPlayer.Identifier = value.Identifier;                    
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
                    dbPlayer.Identifier = value.Identifier;
                }
            }

            _playerContext.SaveChanges();
        }

        public void IngestMatchData()
        {
            Console.WriteLine("Match data ingestion started");
            var directoryInfo = new DirectoryInfo(_generalSettings.InputFolderPath);

            var directoryInfos = directoryInfo.GetDirectories();
            var jsonParser = new JsonParser.JsonParser();
            var elasticClient = new ElasticClientProvider().GetElasticClient(_generalSettings.ElasticUrl);
            string indexName;


            foreach (var directory in directoryInfos)
            {
                var files = directory.GetFiles("*.json");

                indexName = directory.Name;
                elasticClient.Indices.Delete(indexName);
                Console.WriteLine($"Creating index : {indexName}");


                foreach (var file in files)
                {
                    var match = jsonParser.Parse(file.FullName);

                    var isBothTeamsExcluded = match.Info.Teams.All(s => _excludedTeams.Any(t => t.TeamName.Equals(s, StringComparison.InvariantCultureIgnoreCase)));

                    if (isBothTeamsExcluded)
                    {
                        continue;
                    }

                    var balls = new List<Elastic.Ball>();

                    var matchId = Guid.NewGuid().ToString();                   

                    for (int i = 0; i < match.Innings.Count; i++)
                    {
                        var inning = match.Innings[i];
                        var battingTeam = inning.Team;
                        var bowlingTeam = match.Info.Teams.First(t => !t.Equals(inning.Team));

                        var inningsId = Guid.NewGuid().ToString();                       

                        foreach (var over in inning.Overs)
                        {
                            var overId = Guid.NewGuid().ToString();
                            for (int j = 0; j < over.Deliveries.Length; j++)
                            {
                                var ballNumber = j + 1;

                                var delivery = over.Deliveries[j];
                                var ball = _mapper.Map<Elastic.Ball>(delivery);
                                var bowler = GetPlayer(match.Players[delivery.Bowler]);
                                var elasticBowler = _mapper.Map<Elastic.Player>(bowler);
                                elasticBowler.Team = bowlingTeam;
                                ball.Bowler = elasticBowler;

                                var batsman = GetPlayer(match.Players[delivery.Batter]); ;
                                var elasticBatsman = _mapper.Map<Elastic.Player>(batsman);
                                elasticBatsman.Team = battingTeam;
                                ball.Batsman = elasticBatsman;

                                var nonStriker = GetPlayer(match.Players[delivery.NonStriker]); ;
                                var elasticNonStriker = _mapper.Map<Elastic.Player>(nonStriker);
                                elasticNonStriker.Team = battingTeam;
                                ball.NonStriker = elasticNonStriker;

                                var innings = _mapper.Map<Elastic.Inning>(inning);
                                innings.Number = i + 1;
                                ball.Inning = innings;

                                ball.BallNumber = ballNumber;
                                ball.BowlingTeam = bowlingTeam;
                                ball.BattingTeam = battingTeam;
                                ball.MatchId = matchId;
                                ball.InningsId = inningsId;
                                ball.OverId = overId;
                                ball.IsPowerPlay = IsPowerPlay(ballNumber,over.OverNumber, inning);
                                ball.IsSuperOver = inning.SuperOver;
                                ball.Date = Convert.ToDateTime(match.Info.Dates.First());
                                _mapper.Map(over, ball);
                                _mapper.Map(match.Info, ball);
                                _mapper.Map(inning, ball);
                                _mapper.Map(delivery.Review, ball);

                                var ballMatch = _mapper.Map<Elastic.Match>(match.Info);
                                ball.Match = ballMatch;
                                PopulateWicketPlayers(match, ball);

                                balls.Add(ball);
                            }
                        }
                    }

                    var bulkIndexResponse = elasticClient.Bulk(b => b.Index(indexName)
                                                                .IndexMany(balls));
                }
            }

            Console.WriteLine("Elastic Ingestion complete");
        }

        private bool IsPowerPlay(int ballNumber, int overNumber, Innings inning)
        {
            if(inning.Powerplays == null)
            {
                return false;
            }

            var ball = ToDouble(overNumber, ballNumber);
            foreach (var powerplay in inning.Powerplays)
            {
                if (ball >= powerplay.From && ball <= powerplay.To)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetFrac2Digits(decimal d)
        {
            var str = d.ToString("0.0", CultureInfo.InvariantCulture);
            return int.Parse(str[(str.IndexOf('.') + 1)..]);
        }

        public double ToDouble(int left, int right)
        {
            return double.Parse(left.ToString() + "." + right.ToString(),  CultureInfo.InvariantCulture);
        }

        private void PopulateWicketPlayers(CricketMatch match, Elastic.Ball ball)
        {
            foreach (var wicket in ball.Wickets)
            {
                wicket.PlayerOutPlayer = _mapper.Map<Elastic.Player>(GetPlayer(match.Players[wicket.PlayerOut]));
                foreach (var fielder in wicket.Fielders)
                {
                    fielder.FielderPlayer = _mapper.Map<Elastic.Player>(GetPlayer(match.Players[fielder.Name]));
                }
            }
        }

        private Player GetPlayer(MatchPlayer matchPlayer)
        {
            
            try
            {
                Player player = _dbPlayers.Single(p => p.Identifier != null && p.Identifier.Equals(matchPlayer.Identifier, StringComparison.InvariantCultureIgnoreCase));
                //if (player == null)
                //{
                //    var playerAliasMapping
                //        = _playerAliases.FirstOrDefault(p => p.CricsheetName.Equals(playerName, StringComparison.InvariantCultureIgnoreCase));

                //    return _dbPlayers.Single(p => p.CricInfoId == playerAliasMapping.CricInfoId);
                //}

                return player;
            }
            catch(Exception e)
            {
                Console.WriteLine($"PlayerName failed : {matchPlayer.Name}, {matchPlayer.Identifier}. Exception: {e.Message}");
                throw;
            }
        }
    }
}
