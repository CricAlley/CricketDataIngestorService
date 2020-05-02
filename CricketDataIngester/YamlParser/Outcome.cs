using YamlDotNet.Serialization;

namespace CricketDataIngester.YamlParser
{
    public class Outcome
    {
        public string Winner { get; set; }

        public By By { get; set; }

        [YamlMember(Alias = "bowl_out", ApplyNamingConventions = false)]
        public string BowloutWinner { get; set; }

        [YamlMember(Alias = "eliminator", ApplyNamingConventions = false)]
        public string EliminatorWinner { get; set; }

        public string Method { get; set; }

        public string Result { get; set; }
    }
}