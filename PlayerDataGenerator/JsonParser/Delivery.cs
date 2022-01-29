using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Delivery
    {
        [JsonProperty("batter")]
        public string Batter { get; set; }

        [JsonProperty("bowler")]
        public string Bowler { get; set; }

        [JsonProperty("non_striker")]
        public string NonStriker { get; set; }

        [JsonProperty("runs")]
        public Runs Runs { get; set; }

        [JsonProperty("wickets")]
        public List<Wicket> Wickets { get; set; }

        [JsonProperty("replacements")]
        public Replacements Replacements { get; set; }

        [JsonProperty("review")]
        public Review Review { get; set; }

        [JsonProperty("extras")]
        public Extras Extras { get; set; }
    }
}
