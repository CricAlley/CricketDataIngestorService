using Newtonsoft.Json;

namespace PlayerDataGenerator.JsonParser
{
    public class Target
    {
        [JsonProperty("overs")]    
        public double Overs { get; set; }

        [JsonProperty("runs")]
        public int Runs { get; set; }
    }
}
