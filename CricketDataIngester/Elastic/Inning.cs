using System.Collections.Generic;
using CricketDataIngester.YamlParser;

namespace CricketDataIngester.Elastic
{
    public class Inning
    {
        public string BowlingTeam { get; set; }
        public string BattingTeam { get; set; }
        public int InningNumber { get; set; }
        public List<string> AbsentHurt { get; set; }
        public PenaltyRuns PenaltyRuns { get; set; }
        public int IsDeclared { get; set; }
    }
}