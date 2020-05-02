using System.Collections.Generic;

namespace CricketDataIngester.YamlParser
{
    public class Replacements
    {
        public List<ReplacementRole> Role { get; set; }
        public List<ReplacementMatch> Match { get; set; }
    }
}