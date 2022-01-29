using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Registry
    {
        [JsonProperty("people")]
        public Dictionary<string, string> People { get; set; }
    }
}
