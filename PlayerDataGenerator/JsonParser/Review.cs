using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Review
    {
        [JsonProperty("by")]
        public string By { get; set; }

        [JsonProperty("umpire")]
        public string Umpire { get; set; }

        [JsonProperty("batter")]
        public string Batter { get; set; }

        [JsonProperty("decision")]
        public string Decision { get; set; }

        [JsonProperty("umpires_call")]
        public bool UmpiresCall { get; set; }
    }
}