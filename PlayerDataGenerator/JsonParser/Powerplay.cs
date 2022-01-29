using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Powerplay
    {
        [JsonProperty("from")]
        public double From { get; set; }

        [JsonProperty("to")]
        public double To { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
    }


}
