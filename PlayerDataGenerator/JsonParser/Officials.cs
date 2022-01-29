using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Officials
    {
        [JsonProperty("match_referees")]
        public List<string> MatchReferees { get; set; } = new List<string>();

        [JsonProperty("reserve_umpires")]
        public List<string> ReserveUmpires { get; set; } = new List<string>();

        [JsonProperty("tv_umpires")]
        public List<string> TvUmpires { get; set; } = new List<string>();

        [JsonProperty("umpires")]
        public List<string> Umpires { get; set; } = new List<string>();

        public bool Contains (string name)
        {
            if (MatchReferees.Contains(name))
            {
                return true;
            }

            if (ReserveUmpires.Contains(name))
            {
                return true;
            }

            if (TvUmpires.Contains(name))
            {
                return true;
            }

            if (Umpires.Contains(name))
            {
                return true;
            }

            return false;
        }
    }
}
