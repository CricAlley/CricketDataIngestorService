using YamlDotNet.Serialization;

namespace CricketDataIngester.YamlParser
{
    public class Extras   
    {
        public int Wides { get; set; }
        [YamlMember(Alias = "legbyes")]
        public int LegByes { get; set; }
        public int Byes { get; set; }
        public int Penalty { get; set; }
        [YamlMember(Alias = "noballs")]

        public int NoBalls { get; set; }
    }
}