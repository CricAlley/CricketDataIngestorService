using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Innings
    {
        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("overs")]
        public List<Over> Overs { get; set; }

        [JsonProperty("powerplays")]
        public List<Powerplay> Powerplays { get; set; }

        [JsonProperty("target")]
        public Target Target { get; set; }

        [JsonProperty("super_over")]
        public bool SuperOver { get; set; }

        [JsonProperty("miscounted_overs")]
        public Dictionary<string, MiscountedOvers> MiscountedOvers { get; set; }

        [JsonProperty("penalty_runs")]
        public PenaltyRuns PenaltyRuns { get; set; }

        [JsonProperty("absent_hurt")]
        public List<string> AbsentHurt { get; set; }

        [JsonProperty("declared")]
        public bool Declared { get; set; }
    }
}
