using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class MiscountedOvers
    {
        [JsonProperty("balls")]
        public int Balls { get; set; }

        [JsonProperty("umpire")]
        public string Umpire { get; set; }
    }
}