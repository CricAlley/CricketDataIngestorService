﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PlayerDataGenerator.Data;

namespace PlayerDataGenerator
{
    internal interface IPlayerExtractor
    {
        void Start();
    }

    internal class PlayerExtractor : IPlayerExtractor
    {
        private const string BATSMAN = "Batsman";
        private const string BOWLER = "Bowler";
        private const string ALLROUNDER = "AllRounder";
        private const string WICKET_KEEPER = "WicketKeeper";
        private readonly GeneralSettings _generalSettings;
        private readonly PlayerContext _playerContext;
        private readonly List<Tuple<string, int>> _preloadedPlayers;
        private readonly Dictionary<string, Player> _players;
        private readonly Dictionary<string, string> _failedPlayers =  new Dictionary<string, string>();
        private readonly Dictionary<string, Player> _unavailablePlayers =  new Dictionary<string, Player>();

        public PlayerExtractor(GeneralSettings generalSettings, PlayerContext playerContext)
        {
            _generalSettings = generalSettings;
            _playerContext = playerContext;
            _players = new Dictionary<string, Player>();
            _preloadedPlayers = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("A Singh", 26789),
                new Tuple<string, int>("Jaskaran Singh", 376102),
                new Tuple<string, int>("R Sharma", 272994),
                new Tuple<string, int>("N Saini", 274785),
                new Tuple<string, int>("R Shukla", 390547),
                new Tuple<string, int>("GS Sandhu", 499660),
                new Tuple<string, int>("HTRY Thornton", 837611),
                new Tuple<string, int>("Shadab Khan", 922943),
                new Tuple<string, int>("D Morton", 922943),
                new Tuple<string, int>("Rashid Khan", 793463),
                new Tuple<string, int>("J Edwards", 1088610),
                new Tuple<string, int>("C Green", 1076713),
                new Tuple<string, int>("TL O''Connell", 1159591),
                new Tuple<string, int>("SJ Coyte (2)", 4886),
                new Tuple<string, int>("MJ Buchanan", 505118),
                new Tuple<string, int>("OA Shah", 20123),
                new Tuple<string, int>("Yasir Arafat", 43654),
                new Tuple<string, int>("TM Dilshan", 48472),
                new Tuple<string, int>("Mohammad Hafeez", 41434),
                new Tuple<string, int>("DPMD Jayawardene", 49289),
            };

            _unavailablePlayers.Add("AG Harriott",
                new Player
                {
                    FullName = "Andrew G Harriott",
                    Name = "Andrew Harriott",
                    BattingStyle = "Right-hand bat",
                    DateOfBirth = null,
                    CricInfoId = 437446,
                    CricsheetName = "AG Harriott",
                    IsActive = true,
                    PlayingRole = BATSMAN
                });

            _unavailablePlayers.Add("H Kerr",
                new Player
                {
                    CricInfoId = 1163855,
                    CricsheetName = "H Kerr",
                    Name = "Hayden Kerr",
                    FullName = "Hayden Kerr",
                    DateOfBirth = new DateTime(1996, 07, 10),
                    BattingStyle = "Right-hand bat",
                    BowlingStyle = "Left-arm fast-medium",
                    PlayingRole = ALLROUNDER,
                    IsActive = true,
                });

            _unavailablePlayers.Add("NT Ellis",
                new Player
                {
                    CricInfoId = 826915,
                    CricsheetName = "NT Ellis",
                    Name = "Nathan Ellis",
                    FullName = "Nathan Ellis",
                    DateOfBirth = new DateTime(1994, 09, 22),
                    BattingStyle = "Right-hand bat",
                    BowlingStyle = "Right-arm medium",
                    PlayingRole = ALLROUNDER,
                    IsActive = true,
                });

            _unavailablePlayers.Add("LR Morris",
                new Player
                {
                    CricInfoId = 1125317,
                    CricsheetName = "LR Morris",
                    Name = "Lance Morris",
                    FullName = "Lance R Morris",
                    DateOfBirth = new DateTime(1998, 03, 28),
                    BattingStyle = "Right-hand bat",
                    BowlingStyle = "Right-arm medium",
                    PlayingRole = BOWLER,
                    IsActive = true,
                });

            _unavailablePlayers.Add("J Morgan",
                new Player
                {
                    CricInfoId = 605573,
                    CricsheetName = "J Morgan",
                    Name = "Jaron Morgan",
                    FullName = "Jaron Morgan",
                    DateOfBirth = new DateTime(1995, 09, 27),
                    BattingStyle = "Left-hand bat",
                    BowlingStyle = "Legbreak",
                    PlayingRole = BATSMAN,
                    IsActive = true,
                });

            _unavailablePlayers.Add("NA McSweeney",
                new Player
                {
                    CricInfoId = 1124290,
                    CricsheetName = "NA McSweeney",
                    Name = "Nathan McSweeney",
                    FullName = "Nathan A McSweeney",
                    DateOfBirth = new DateTime(1999, 03, 08),
                    BattingStyle = "Right-hand bat",
                    BowlingStyle = "Right-arm offbreak",
                    PlayingRole = BOWLER,
                    IsActive = true,
                });

            _unavailablePlayers.Add("Dilbar Hussain",
                new Player
                {
                    CricInfoId = 1206623,
                    CricsheetName = "Dilbar Hussain",
                    Name = "Dilbar Hussain",
                    FullName = "Dilbar Hussain",
                    DateOfBirth = new DateTime(1993, 02, 20),
                    BattingStyle = "Right-hand bat",
                    BowlingStyle = "Right-arm fast-medium",
                    PlayingRole = BOWLER,
                    IsActive = true,
                });

            _unavailablePlayers.Add("Z Evans",
                new Player
                {
                    CricInfoId = 1088612,
                    CricsheetName = "Z Evans",
                    Name = "Zak Evans",
                    FullName = "Zak Evans",
                    DateOfBirth = new DateTime(2000, 03, 26),
                    BattingStyle = "Right-hand bat",
                    BowlingStyle = "Right-arm medium-fast",
                    PlayingRole = BOWLER,
                    IsActive = true,
                });

            _unavailablePlayers.Add("PJ Hughes",
                new Player
                {
                    CricInfoId = 272364,
                    CricsheetName = "PJ Hughes",
                    Name = "Phillip Hughes",
                    FullName = "Phillip Joel Hughes",
                    DateOfBirth = new DateTime(1988, 11, 30),
                    BattingStyle = "Left-hand bat",
                    BowlingStyle = null,
                    PlayingRole = BATSMAN,
                    IsActive = true,
                });

            _unavailablePlayers.Add("RR Ayre",
                new Player
                {
                    CricInfoId = 638911,
                    CricsheetName = "RR Ayre",
                    Name = "Riley Ayre",
                    FullName = "Riley R Ayre",
                    DateOfBirth = new DateTime(1996, 04, 02),
                    BattingStyle = "Right-hand bat",
                    BowlingStyle = "Slow left-arm orthodox",
                    PlayingRole = BOWLER,
                    IsActive = true,
                });

            foreach (var plr in _preloadedPlayers)
            {
                var player = _playerContext.Players.First(p => p.CricInfoId == plr.Item2);

                player.CricsheetName = plr.Item1;

                _players.Add(plr.Item1, player);
            }

            foreach (var unavailablePlayer in _unavailablePlayers)
            {
                _playerContext.Players.Add(unavailablePlayer.Value);
                _players.Add(unavailablePlayer.Key, unavailablePlayer.Value);
            }

            _playerContext.SaveChanges();
        }

        public void Start()
        {
            Console.WriteLine("Player Data Extraction started");
            var directoryInfo = new DirectoryInfo(_generalSettings.InputFolderPath);

            var directoryInfos = directoryInfo.GetDirectories();
            var yamlParser = new YamlParser.YamlParser();
            var stringBuilder = new StringBuilder();

            int count = 0;

            foreach (var directory in directoryInfos)
            {
                var files = directory.GetFiles("*.yaml");

                foreach (var file in files)
                {
                    var match = yamlParser.Parse(file.FullName);

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
                                $"exception: {e.Message}, Player Name: {player} + yaml- {file.Name}, matchDate - {match.MatchInfo.Dates}, matchTeams - {string.Join("-", match.MatchInfo.Teams)} ");
                        }

                    }
                }
            }

            stringBuilder.Append($"Unable to Map {count} Players ");

            File.WriteAllText("PlayersData.sql", stringBuilder.ToString());
            Console.WriteLine("Extraction Complete");

            if(_failedPlayers.Count> 0)
                throw new Exception($"there are {count} unmapped players. Please Map them.");
            Console.ReadKey();
        }

        private void UpdatePlayer(string player)
        {
            player = player.Replace(" (sub)", "");

            if (_players.ContainsKey(player) || _failedPlayers.ContainsKey(player)) return;

            List<Player> foundPlayers = new List<Player>();

            var names = player.Split(' ');

            var lastName = names.Last();
            var firstName = names.First();
            var isInitials = firstName.ToCharArray().All(c => c == char.ToUpper(c));

            var middleName = string.Empty;
            if (names.Length > 2) middleName = names.Skip(1).First();

            if (isInitials)
            {
                var dbPlayers = _playerContext.Players.Where(player1 =>
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
                    if (dbLastName.ToUpper() == lastName.ToUpper())
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
                            isfound = dbFirstName == firstName;
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
                var dbPlayers = _playerContext.Players.Where(player1 =>
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

                    if (firstName.ToUpper() == dbFirstName.ToUpper() &&
                        lastName.ToUpper() == dbLastName.ToUpper() &&
                        middleName?.ToUpper() == dbMiddleName?.ToUpper())
                    {
                        foundPlayers.Add(dbPlayer);
                    }
                }

                if (!foundPlayers.Any())
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
