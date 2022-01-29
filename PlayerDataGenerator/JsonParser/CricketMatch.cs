using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlayerDataGenerator.JsonParser
{
    public class CricketMatch
    {
        private Dictionary<string, Player> _player;
        public Meta Meta { get; set; }
        public Info Info { get; set; }
        public List<Innings> Innings { get; set; }

        [JsonIgnore]
        public Dictionary<string, Player> Players
        {
            get
            {
                if(_player == null)
                {
                    _player = GetPlayers();
                }

                return _player;
            }
        }

        private Dictionary<string, Player> GetPlayers()
        {
            var teamPlayers = new Dictionary<string, Player>();

            foreach (var people in Info.Registry.People)
            {
                var player = new Player { Name = people.Key, Uid = people.Value, Team = GetTeam(people.Key) };
                teamPlayers.Add(player.Name, player);
            }            

            return teamPlayers;
        }

        private string GetTeam(string playerName)
        {
            foreach (var team in Info.Players)
            {
                if (team.Value.Contains(playerName))
                {
                    return team.Key;
                }
            }

            return null;
        }
    }
}
