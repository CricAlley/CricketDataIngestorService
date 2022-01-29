using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Event
    {
        [JsonProperty("match_number")]
        public int MatchNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("sub_name")]
        public string SubName { get; set; }
    }


}
