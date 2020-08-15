using System;
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
        private readonly Dictionary<string, string> _failedPlayers = new Dictionary<string, string>();
        private readonly Dictionary<string, int> _playerAliases = new Dictionary<string, int>();
        private readonly Dictionary<string, Player> _unavailablePlayers = new Dictionary<string, Player>();
        protected List<string> _excludedTeams = new List<string>();
        protected List<string> _teams = new List<string>();

        public PlayerExtractor(GeneralSettings generalSettings, PlayerContext playerContext)
        {
            _generalSettings = generalSettings;
            _playerContext = playerContext;
            _players = new Dictionary<string, Player>();

            //bbl_male
            //ipl_male
            //ntb_male
            //odis_male
            //t20s_male

            _excludedTeams.Add("Nottinghamshire");
            _excludedTeams.Add("Worcestershire");
            _excludedTeams.Add("Glamorgan");
            _excludedTeams.Add("Somerset");
            _excludedTeams.Add("Durham");
            _excludedTeams.Add("Northamptonshire");
            _excludedTeams.Add("Essex");
            _excludedTeams.Add("Surrey");
            _excludedTeams.Add("Gloucestershire");
            _excludedTeams.Add("Kent");
            _excludedTeams.Add("Derbyshire");
            _excludedTeams.Add("Yorkshire");
            _excludedTeams.Add("Lancashire");
            _excludedTeams.Add("Birmingham Bears");
            _excludedTeams.Add("Leicestershire");
            _excludedTeams.Add("Hampshire");
            _excludedTeams.Add("Middlesex");
            _excludedTeams.Add("Sussex");
            _excludedTeams.Add("Warwickshire");
            _excludedTeams.Add("Scotland");
            _excludedTeams.Add("Afghanistan");
            _excludedTeams.Add("Hong Kong");
            _excludedTeams.Add("Zimbabwe");
            _excludedTeams.Add("Papua New Guinea");
            _excludedTeams.Add("United Arab Emirates");
            _excludedTeams.Add("Nepal");
            _excludedTeams.Add("United States of America");
            _excludedTeams.Add("Namibia");
            _excludedTeams.Add("Oman");
            _excludedTeams.Add("Netherlands");
            _excludedTeams.Add("Bermuda");
            _excludedTeams.Add("Canada");
            _excludedTeams.Add("Kenya");
            _excludedTeams.Add("Africa XI");
            _excludedTeams.Add("Asia XI");
            _excludedTeams.Add("Philippines");
            _excludedTeams.Add("Vanuatu");
            _excludedTeams.Add("Germany");
            _excludedTeams.Add("Italy");
            _excludedTeams.Add("Ghana");
            _excludedTeams.Add("Uganda");
            _excludedTeams.Add("Botswana");
            _excludedTeams.Add("Nigeria");
            _excludedTeams.Add("Guernsey");
            _excludedTeams.Add("Denmark");
            _excludedTeams.Add("Norway");
            _excludedTeams.Add("Jersey");
            _excludedTeams.Add("Malaysia");
            _excludedTeams.Add("Thailand");
            _excludedTeams.Add("Maldives");
            _excludedTeams.Add("Singapore");
            _excludedTeams.Add("Qatar");
            _excludedTeams.Add("Kuwait");
            _excludedTeams.Add("Cayman Islands");
            _excludedTeams.Add("Spain");
            _excludedTeams.Add("Portugal");
            _excludedTeams.Add("Gibraltar");
            _excludedTeams.Add("Bhutan");
            _excludedTeams.Add("Saudi Arabia");
            _excludedTeams.Add("Bahrain");
            _excludedTeams.Add("Iran");
            _excludedTeams.Add("Ireland");
            _excludedTeams.Add("Zimbabwe");


            _preloadedPlayers = new List<Tuple<string, int>>
            {
                //new Tuple<string, int>("A Singh", 26789),
            };

            _playerAliases.Add("Martyn", 6513); // Damien martyn
            _playerAliases.Add("Kapugedera", 209457); 
            _playerAliases.Add("Astle", 36185);
            _playerAliases.Add("Fahim Ashraf", 681117);
            _playerAliases.Add("P Young-Husband", 707183);
            _playerAliases.Add("MDK Perera", 49920);

            //_unavailablePlayers.Add("MDK Perera",
            //   new Player
            //   {
            //       CricInfoId = 49920,
            //       CricsheetName = "MDK Perera",
            //       Name = "Dilruwan Perera",
            //       FullName = "Mahawaduge Dilruwan Kamalaneth Perera",
            //       DateOfBirth = new DateTime(1982, 07, 22),
            //       BattingStyle = "Right-hand bat",
            //       BowlingStyle = "Right-arm offbreak",
            //       PlayingRole = ALLROUNDER,
            //       IsActive = true,
            //   });       



            foreach (var plr in _preloadedPlayers)
            {
                var player = _playerContext.Players.First(p => p.CricInfoId == plr.Item2);

                player.CricsheetName = plr.Item1;

                _players.Add(plr.Item1, player);
            }

            foreach(var unavailablePlayer in _unavailablePlayers)
            {
                if(_playerContext.Players.FirstOrDefault(p => p.CricInfoId == unavailablePlayer.Value.CricInfoId) == null)
                {
                    _playerContext.Players.Add(unavailablePlayer.Value);
                }

                _players.TryAdd(unavailablePlayer.Key, unavailablePlayer.Value);

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
            var teamsBuilder = new StringBuilder();

            int count = 0;

            foreach(var directory in directoryInfos)
            {
                var files = directory.GetFiles("*.yaml");

                foreach(var file in files)
                {
                    var match = yamlParser.Parse(file.FullName);

                    var isBothTeamsExcluded = match.MatchInfo.Teams.All(s => _excludedTeams.Any(t => t == s));

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
                throw new Exception($"there are {count} unmapped players. Please Map them.");
            else
                GeneratePlayerDataScript();
            Console.ReadKey();
        }

        private void GeneratePlayerDataScript()
        {
            var players = _playerContext.Players.ToList();

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("IF OBJECT_ID('tempdb..#players') IS NOT NULL");
            stringBuilder.AppendLine("  DROP TABLE #players");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("CREATE TABLE #players");
            stringBuilder.AppendLine("(");
            stringBuilder.AppendLine("  [Id]            INT             IDENTITY(1, 1) NOT NULL,");
            stringBuilder.AppendLine("  [Name]          NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [FullName]      NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [PlayingRole]   NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [DateOfBirth]   DATETIME        NULL,");
            stringBuilder.AppendLine("  [BattingStyle]  NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [BowlingStyle]  NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [CricInfoId]    INT             NOT NULL,");
            stringBuilder.AppendLine("  [IsActive]      BIT             DEFAULT((1)),");
            stringBuilder.AppendLine("  [CricsheetName] NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine(");");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(
                "INSERT INTO #players	([Name], [FullName], [PlayingRole], [DateOfBirth], [BattingStyle], [BowlingStyle], [CricInfoId], [IsActive], [CricsheetName])");

            var lastPlayerIndex = players.Count() - 1;

            for (var index = 0; index < players.Count(); index++)
            {
                var player = players[index];

                var canSplitBatch = index % 1000 == 0;
                var cricSheetName = player.CricsheetName== null ? "NULL" : $"'{player.CricsheetName.Replace("'", "''")}'";
                var dateOfBirth = player.DateOfBirth== null ? "NULL" : $"'{player.DateOfBirth}'";
                if (canSplitBatch)
                {
                    
                    stringBuilder.AppendLine(
                        $@"SELECT '{player.Name.Replace("'", "''")}' AS Name, '{player.FullName.Replace("'", "''")}' AS FullName, '{player.PlayingRole}' AS PlayingRole, {dateOfBirth} AS DateofBirth," +
                        $@" '{player.BattingStyle}' AS BattingStyle, '{player.BowlingStyle}' AS BowlingStyle, '{player.CricInfoId}' AS CricInfoId, 1 AS IsActive, {cricSheetName} AS CricsheetName");
                    
                    stringBuilder.AppendLine("Go");

                    stringBuilder.AppendLine();

                    stringBuilder.AppendLine(
                        "INSERT INTO #players	([Name], [FullName], [PlayingRole], [DateOfBirth], [BattingStyle], [BowlingStyle], [CricInfoId], [IsActive], [CricsheetName])");
                }
                else if (index == lastPlayerIndex)
                {
                    
                    stringBuilder.AppendLine(
                        $@"SELECT '{player.Name.Replace("'", "''")}' AS Name, '{player.FullName.Replace("'", "''")}' AS FullName, '{player.PlayingRole}' AS PlayingRole, {dateOfBirth} AS DateofBirth," +
                        $@" '{player.BattingStyle}' AS BattingStyle, '{player.BowlingStyle}' AS BowlingStyle, '{player.CricInfoId}' AS CricInfoId, 1 AS IsActive, {cricSheetName} AS CricsheetName");
                    
                }
                else
                {                    
                    stringBuilder.AppendLine(
                        $@"SELECT '{player.Name.Replace("'", "''")}' AS Name, '{player.FullName.Replace("'", "''")}' AS FullName, '{player.PlayingRole}' AS PlayingRole, {dateOfBirth}  AS DateofBirth," +
                        $@" '{player.BattingStyle}' AS BattingStyle, '{player.BowlingStyle}' AS BowlingStyle, '{player.CricInfoId}' AS CricInfoId, 1 AS IsActive, {cricSheetName} AS CricsheetName UNION ALL");                    
                }
            }

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("BEGIN TRY");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("  BEGIN TRANSACTION");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("      MERGE  dbo.players AS TARGET");
            stringBuilder.AppendLine("          USING #players AS SOURCE");
            stringBuilder.AppendLine("          ON(TARGET.CricInfoId = SOURCE.CricInfoId)");
            stringBuilder.AppendLine("              WHEN MATCHED AND TARGET.PlayingRole <> SOURCE.PlayingRole");
            stringBuilder.AppendLine("              THEN");
            stringBuilder.AppendLine("                  UPDATE");
            stringBuilder.AppendLine("                  SET TARGET.PlayingRole = SOURCE.PlayingRole");
            stringBuilder.AppendLine("              WHEN NOT MATCHED BY TARGET");
            stringBuilder.AppendLine("              THEN");
            stringBuilder.AppendLine("                  INSERT(Name");
            stringBuilder.AppendLine("                        , FullName");
            stringBuilder.AppendLine("                        , PlayingRole");
            stringBuilder.AppendLine("                        , DateOfBirth");
            stringBuilder.AppendLine("                        , BattingStyle");
            stringBuilder.AppendLine("                        , BowlingStyle");
            stringBuilder.AppendLine("                        , CricInfoId");
            stringBuilder.AppendLine("                        , IsActive");
            stringBuilder.AppendLine("                        , CricsheetName)");
            stringBuilder.AppendLine("                  VALUES(SOURCE.Name");
            stringBuilder.AppendLine("                        , SOURCE.FullName");
            stringBuilder.AppendLine("                        , SOURCE.PlayingRole");
            stringBuilder.AppendLine("                        , SOURCE.DateOfBirth");
            stringBuilder.AppendLine("                        , SOURCE.BattingStyle");
            stringBuilder.AppendLine("                        , SOURCE.BowlingStyle");
            stringBuilder.AppendLine("                        , SOURCE.CricInfoId");
            stringBuilder.AppendLine("                        , SOURCE.IsActive");
            stringBuilder.AppendLine("                        , SOURCE.CricsheetName);");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("  COMMIT TRANSACTION");
            stringBuilder.AppendLine("PRINT 'MERGE dbo.Player - Done'");
            stringBuilder.AppendLine("END TRY");
            stringBuilder.AppendLine("BEGIN CATCH");
            stringBuilder.AppendLine("      ROLLBACK TRANSACTION;");
            stringBuilder.AppendLine("      THROW");
            stringBuilder.AppendLine("END CATCH");

            File.WriteAllText("PlayersData.sql", stringBuilder.ToString());

            Console.WriteLine("Player Data script is generated.");
        }

        private void UpdatePlayer(string player)
        {
            player = player.Replace(" (sub)", "");
            Player p = _playerContext.Players.SingleOrDefault(p => p.CricsheetName.Equals(player));

            if (p != null) return;

            if(_players.ContainsKey(player) || _failedPlayers.ContainsKey(player) || _playerAliases.ContainsKey(player)) return;

            List<Player> foundPlayers = new List<Player>();

            var names = player.Split(' ');

            var lastName = names.Last();
            var firstName = names.First();
            var isInitials = firstName.ToCharArray().All(c => c == char.ToUpper(c));

            var middleName = string.Empty;
            if(names.Length > 2) middleName = names.Skip(1).First();

            if(isInitials)
            {
                var dbPlayers = _playerContext.Players.Where(player1 =>
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
                var dbPlayers = _playerContext.Players.Where(player1 =>
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
