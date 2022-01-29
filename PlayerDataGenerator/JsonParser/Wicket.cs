using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Wicket
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("player_out")]
        public string PlayerOut { get; set; }

        [JsonProperty("fielders")]
        public List<Fielder> Fielders { get; set; }
    }
}
