using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Fielder
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("substitute")]
        public bool Substitute  { get; set; }
    }


}
