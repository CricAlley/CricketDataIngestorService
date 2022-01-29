using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Outcome
    {
        [JsonProperty("by")]
        public By By { get; set; }

        [JsonProperty("winner")]
        public string Winner { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("eliminator")]
        public string Eliminator { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("bowl_out")]
        public string BowlOut { get; set; }
    }


}
