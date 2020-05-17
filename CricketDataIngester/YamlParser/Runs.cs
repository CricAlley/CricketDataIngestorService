using YamlDotNet.Serialization;

namespace CricketDataIngester.YamlParser
{
    public class Runs
    {
        public int Extras { get; set; }
        public int Total { get; set; }
        public int Batsman { get; set; }

        [YamlMember(Alias = "non_boundary", ApplyNamingConventions = false)]
        public int IsNonBoundary { get; set; }
    }
}