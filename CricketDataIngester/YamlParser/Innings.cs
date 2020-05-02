using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace CricketDataIngester.YamlParser
{
    public class Inning
    {
        public string Team { get; set; }

        public List<Dictionary<decimal, Deliveries>> Deliveries { get; set; }

        [YamlMember(Alias = "absent_hurt", ApplyNamingConventions = false)]
        public List<string> AbsentHurt { get; set; }

        [YamlMember(Alias = "penalty_runs", ApplyNamingConventions = false)]
        public PenaltyRuns PenaltyRuns { get; set; }
        [YamlMember(Alias = "declared", ApplyNamingConventions = false)]
        public int IsDeclared { get; set; }
    }
}