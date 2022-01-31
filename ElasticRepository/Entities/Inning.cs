using System.Collections.Generic;

namespace ElasticRepository.Entities
{
    public class Inning
    {
        public string Team { get; set; }
        public int Number { get; set; }
        public List<string> AbsentHurt { get; set; }
        public PenaltyRuns PenaltyRuns { get; set; }
        public bool IsDeclared { get; set; }

        public Target Target { get; set; }
    }
}