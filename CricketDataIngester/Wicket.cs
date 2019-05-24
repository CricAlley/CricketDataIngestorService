using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace CricketDataIngester
{
    public class Wicket
    {
        [YamlMember(Alias = "player_out", ApplyNamingConventions = false)]
        public string PlayerOut { get; set; }
        public List<string> Fielders { get; set; }
        public string Kind { get; set; }
    }
}