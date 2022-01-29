using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class PenaltyRuns
    {
        [JsonProperty("pre")]
        public int Pre { get; set; }

        [JsonProperty("post")]
        public int Post { get; set; }
    }
}