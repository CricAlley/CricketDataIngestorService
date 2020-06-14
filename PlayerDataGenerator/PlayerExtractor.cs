using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                //new Tuple<string, int>("AG Harriott", 437446),
                new Tuple<string, int>("Shadab Khan", 922943),
                new Tuple<string, int>("D Morton", 922943),
            };

            _unavailablePlayers.Add("AG Harriott",
                new Player()
                {
                    FullName = "Andrew G Harriott", Name = "Andrew Harriott", BattingStyle = "Right-hand bat",
                    CricInfoId = 437446, CricsheetName = "AG Harriott", IsActive = true, PlayingRole = BATSMAN
                });



            foreach (var plr in _preloadedPlayers)
            {
                var player = _playerContext.Players.FirstOrDefault(p => p.CricInfoId == plr.Item2);

                if (player == null)
                {

                }

                player.CricsheetName = plr.Item1;
                _players.Add(plr.Item1, player);
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

            Player foundPlayer = null;

            var names = player.Split(' ');

            var lastName = names.Last();
            var firstName = names.First();
            var isInitials = firstName.ToCharArray().All(c => c == char.ToUpper(c));

            var middleName = string.Empty;
            if (names.Length > 2) middleName = names.Skip(1).First();

            if (isInitials)
            {
                var dbPlayers = _playerContext.Players.Where(player1 =>
                    player1.FullName.ToUpper().Contains(lastName.ToUpper()) && player1.IsActive);

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
                        _players.Add(player, dbPlayer);
                        foundPlayer = dbPlayer;

                        dbPlayer.CricsheetName = player;
                    }
                }

                if (foundPlayer == null)
                {
                    throw new NullReferenceException("Player not found");
                }
            }
            else
            {
                var dbPlayers = _playerContext.Players.Where(player1 =>
                    player1.FullName.ToUpper().Contains(firstName.ToUpper()) && player1.IsActive);

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
                        _players.Add(player, dbPlayer);
                        foundPlayer = dbPlayer;
                        dbPlayer.CricsheetName = player;
                    }
                }

                if (foundPlayer == null) throw new NullReferenceException("Player not found");
            }

            _playerContext.SaveChanges();
        }
    }
}
