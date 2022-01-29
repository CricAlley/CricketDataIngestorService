using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Role
    {
        [JsonProperty("in")]
        public string In { get; set; }

        [JsonProperty("out")]
        public string Out { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("role")]
        public string PlayerRole { get; set; }
    }
}