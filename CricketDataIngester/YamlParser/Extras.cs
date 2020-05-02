using YamlDotNet.Serialization;

namespace CricketDataIngester.YamlParser
{
    public class Extras   
    {
        public int Wides { get; set; }
        [YamlMember(Alias = "legbyes", ApplyNamingConventions = false)]
        public int LegByes { get; set; }
        public int Byes { get; set; }
        public int Penalty { get; set; }
        [YamlMember(Alias = "noballs", ApplyNamingConventions = false)]

        public int NoBalls { get; set; }
    }
}