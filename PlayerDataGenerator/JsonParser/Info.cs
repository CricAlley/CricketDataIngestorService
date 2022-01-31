using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Info
    {
        [JsonProperty("balls_per_over")]
        public int BallsPerOver { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("dates")]
        public List<string> Dates { get; set; }

        [JsonProperty("event")]
        public Event Event { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("match_type")]
        public string MatchType { get; set; }

        [JsonProperty("officials")]
        public Officials Officials { get; set; }

        [JsonProperty("outcome")]
        public Outcome Outcome { get; set; }

        [JsonProperty("overs")]
        public int Overs { get; set; }

        [JsonProperty("player_of_match")]
        public List<string> PlayerOfTheMatch { get; set; }

        [JsonProperty("players")]
        public Dictionary<string, List<string>> Players { get; set; }

        [JsonProperty("registry")]
        public Registry Registry { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("team_type")]
        public string TeamType { get; set; }

        [JsonProperty("teams")]
        public List<string> Teams { get; set; }

        [JsonProperty("toss")]
        public Toss Toss { get; set; }

        [JsonProperty("venue")]
        public string Venue { get; set; }


        [JsonProperty("supersubs")]
        public Dictionary<string,string> Supersubs { get; set; }

        [JsonProperty("missing")]
        [JsonIgnore()]
        public string Missing { get; set; }

        [JsonProperty("match_type_number")]
        public int MatchTypeNumber { get; set; }

        [JsonProperty("bowl_out")]
        public List<BowlOut> BowlOut { get; set; }
    }
}
