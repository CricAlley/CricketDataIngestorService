using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Extras
    {
        [JsonProperty("legbyes")]
        public int LegByes { get; set; }

        [JsonProperty("wides")]
        public int Wides { get; set; }

        [JsonProperty("byes")]
        public int Byes { get; set; }

        [JsonProperty("noballs")]
        public int NoBalls { get; set; }

        [JsonProperty("penalty")]
        public int Penalty { get; set; }
    }
}
