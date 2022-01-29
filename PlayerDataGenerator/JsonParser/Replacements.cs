using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerDataGenerator.JsonParser
{
    public class Replacements
    {
        [JsonProperty("match")]
        public List<Match> Match { get; set; }

        [JsonProperty("role")]
        public List<Role> Role { get; set; }
    }
}