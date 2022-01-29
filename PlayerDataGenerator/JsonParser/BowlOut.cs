using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class BowlOut
    {
        [JsonProperty("bowler")]
        public string Bowler { get; set; }

        [JsonProperty("outcome")]
        public string Outcome { get; set; }
    }
}