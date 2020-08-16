using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PlayerDataGenerator.Data;

namespace PlayerDataGenerator
{
    internal interface ICricketDataIngestor
    {
        void ExtractPlayers();
    }

    internal class CricketDataIngestor : ICricketDataIngestor
    {
        private readonly GeneralSettings _generalSettings;
        private readonly CricketContext _playerContext;
        private readonly IPlayerScriptGenerator _playerScriptGenerator;
        private readonly Dictionary<string, Player> _players;
        private readonly Dictionary<string, string> _failedPlayers = new Dictionary<string, string>();
        private List<ExcludedTeam> _excludedTeams;
        private List<Player> _dbPlayers;
        private List<PlayerAliasMapping> _playerAliases;
        private List<string> _teams = new List<string>();

        public CricketDataIngestor(GeneralSettings generalSettings, CricketContext playerContext, IPlayerScriptGenerator playerScriptGenerator)
        {
            _generalSettings = generalSettings;
            _playerContext = playerContext;
            _playerScriptGenerator = playerScriptGenerator;
            _players = new Dictionary<string, Player>();

            _excludedTeams = playerContext.ExcludedTeams.ToList();
            _dbPlayers = playerContext.Players.ToList();
            _playerAliases = playerContext.PlayerAliasMapping.ToList();

            //bbl_male
            //ipl_male
            //ntb_male
            //odis_male
            //t20s_male  
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

            foreach(var directory in directoryInfos)
            {
                var files = directory.GetFiles("*.yaml");

                foreach(var file in files)
                {
                    var match = yamlParser.Parse(file.FullName);

                    var isBothTeamsExcluded = match.MatchInfo.Teams.All(s => _excludedTeams.Any(t => t.TeamName == s));

                    if(isBothTeamsExcluded)
                    {
                        continue;
                    }

                    foreach(var team in match.MatchInfo.Teams)
                    {
                        if(_teams.All(t => t != team))
                        {
                            teamsBuilder.AppendLine(@"_includedTeams.Add(" + $"{team}" + ");");
                            _teams.Add(team);
                        }
                    }


                    var players = match.GetPlayers();

                    foreach(var player in players)
                    {
                        try
                        {
                            UpdatePlayer(player);
                        }
                        catch(Exception e)
                        {
                            if(_failedPlayers.ContainsKey(player))
                            {
                                continue;
                            }

                            count++;

                            if(_players.ContainsKey(player))
                            {
                                _failedPlayers.Add(player, _players[player].FullName);
                            }
                            else
                            {
                                _failedPlayers.Add(player, player);
                            }

                            stringBuilder.AppendLine(
                                $"exception: {e.Message}, Player Name: {player} + yaml- {file.Name}, matchDate - {match.MatchInfo.Dates.First().ToShortDateString()}, matchTeams - {string.Join("-", match.MatchInfo.Teams)} ");
                        }

                    }
                }
            }

            stringBuilder.Append($"Unable to Map {count} Players ");

            File.WriteAllText("PlayersData.txt", stringBuilder.ToString());
            File.WriteAllText("Teams.txt", teamsBuilder.ToString());
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

        private void UpdatePlayer(string player)
        {
            player = player.Replace(" (sub)", "");
            Player p = _dbPlayers.SingleOrDefault(p => p.CricsheetName != null &&  p.CricsheetName.Equals(player));

            if (p != null) return;

            if(_players.ContainsKey(player) || _failedPlayers.ContainsKey(player) || _playerAliases.Any(pa=> pa.CricsheetName == player)) return;

            List<Player> foundPlayers = new List<Player>();

            var names = player.Split(' ');

            var lastName = names.Last();
            var firstName = names.First();
            var isInitials = firstName.ToCharArray().All(c => c == char.ToUpper(c));

            var middleName = string.Empty;
            if(names.Length > 2) middleName = names.Skip(1).First();

            if(isInitials)
            {
                var dbPlayers = _dbPlayers.Where(player1 =>
                    player1.FullName.ToUpper().Contains(lastName.ToUpper()) && player1.IsActive).ToList();

                if(!dbPlayers.Any())
                {
                    throw new Exception("Player not present");

                }

                foreach(var dbPlayer in dbPlayers)
                {
                    var dbNames = dbPlayer.FullName.Split(' ');
                    var dbLastName = dbNames.Last();
                    var dbFirstName = dbNames.First();
                    var isDbNameInitials = dbFirstName.ToCharArray().All(c => c == char.ToUpper(c));

                    var isfound = false;
                    if(dbLastName.ToUpper() == lastName.ToUpper())
                    {
                        isfound = true;
                        var j = 0;
                        for(var i = 0; i < firstName.Length; i++, j++)
                        {
                            if(dbNames[i][0] != firstName[i])
                            {
                                isfound = false;
                                break;
                            }
                        }

                        if(!string.IsNullOrWhiteSpace(middleName) && dbNames[j].ToUpper() != middleName.ToUpper())
                        {
                            isfound = false;
                        }

                        if(isDbNameInitials)
                        {
                            isfound = dbFirstName == firstName;
                        }
                    }

                    if(isfound)
                    {
                        foundPlayers.Add(dbPlayer);
                    }
                }

                if(!foundPlayers.Any())
                {
                    throw new NullReferenceException("Player not found");
                }
                else if(foundPlayers.Count > 1)
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
                if(!dbPlayers.Any())
                {
                    throw new Exception("Player not present");
                }

                foreach(var dbPlayer in dbPlayers)
                {
                    var dbNames = dbPlayer.Name.Split(' ');
                    var dbFirstName = dbNames.First();
                    var dbLastName = dbNames.Last();
                    var dbMiddleName = string.Empty;
                    if(dbNames.Length > 2) dbMiddleName = dbNames.Skip(1).FirstOrDefault();

                    if(string.Equals(firstName.ToUpper(), dbFirstName.ToUpper(), StringComparison.Ordinal) &&
                        string.Equals(lastName, dbLastName, StringComparison.CurrentCultureIgnoreCase) &&
                        middleName?.ToUpper() == dbMiddleName?.ToUpper())
                    {
                        foundPlayers.Add(dbPlayer);
                    }
                }

                if(!foundPlayers.Any())
                {
                    throw new NullReferenceException("Player not found");
                }
                else if(foundPlayers.Count > 1)
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
    }
}
