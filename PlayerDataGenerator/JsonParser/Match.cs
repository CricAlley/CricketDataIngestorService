using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Match
    {
        [JsonProperty("in")]
        public string In { get; set; }

        [JsonProperty("out")]
        public string Out { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }
    }
}