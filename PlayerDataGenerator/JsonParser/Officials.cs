using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Officials
    {
        [JsonProperty("match_referees")]
        public List<string> MatchReferees { get; set; }

        [JsonProperty("reserve_umpires")]
        public List<string> ReserveUmpires { get; set; }

        [JsonProperty("tv_umpires")]
        public List<string> TvUmpires { get; set; }

        [JsonProperty("umpires")]
        public List<string> Umpires { get; set; }
    }
}
