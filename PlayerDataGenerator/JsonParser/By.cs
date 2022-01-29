using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class By
    {
        [JsonProperty("runs")]
        public int Runs { get; set; }

        [JsonProperty("wickets")]
        public int Wickets { get; set; }

        [JsonProperty("innings")]
        public int Innings { get; set; }
    }


}
