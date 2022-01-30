using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlayerDataGenerator.JsonParser
{
    public class CricketMatch
    {
        private Dictionary<string, MatchPlayer> _player;
        public Meta Meta { get; set; }
        public Info Info { get; set; }
        public List<Innings> Innings { get; set; }

        [JsonIgnore]
        public Dictionary<string, MatchPlayer> Players
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

        private Dictionary<string, MatchPlayer> GetPlayers()
        {
            var teamPlayers = new Dictionary<string, MatchPlayer>();

            foreach (var people in Info.Registry.People)
            {
                if (Info.Officials != null && Info.Officials.Contains(people.Key))
                {
                    continue;
                }

                var player = new MatchPlayer { Name = people.Key, Identifier = people.Value, Team = GetTeam(people.Key) };
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
