using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Toss
    {
        [JsonProperty("decision")]
        public string Decision { get; set; }

        [JsonProperty("winner")]
        public string Winner { get; set; }
    }
}
