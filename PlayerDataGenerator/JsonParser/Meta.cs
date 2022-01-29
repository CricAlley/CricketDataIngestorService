using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Meta
    {
        [JsonProperty("data_version")]
        public string DataVersion { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("revision")]
        public int Revision { get; set; }
    }


}
