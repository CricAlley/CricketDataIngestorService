using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Over
    {
        [JsonProperty("over")]
        public int OverNumber { get; set; }

        [JsonProperty("deliveries")]
        public List<Delivery> Deliveries { get; set; }
    }


}
