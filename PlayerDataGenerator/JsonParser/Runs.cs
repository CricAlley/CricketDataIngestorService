using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Runs
    {
        [JsonProperty("batter")]
        public int Batter { get; set; }

        [JsonProperty("extras")]
        public int Extras { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("non_boundary")]
        public bool NonBoundary { get; set; }
    }
}
