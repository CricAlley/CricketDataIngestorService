using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Over
    {
        [JsonProperty("over")]
        public int OverNumber { get; set; }

        [JsonProperty("deliveries")]
        public Delivery[] Deliveries { get; set; }
    }


}
