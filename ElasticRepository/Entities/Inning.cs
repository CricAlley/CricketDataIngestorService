using System.Collections.Generic;

namespace ElasticRepository.Entities
{
    public class Inning
    {
        public string BowlingTeam { get; set; }
        public string BattingTeam { get; set; }
        public string Innings { get; set; }
        public string InningsId { get; set; }

        public List<string> AbsentHurt { get; set; }
        public PenaltyRuns PenaltyRuns { get; set; }
        public int IsDeclared { get; set; }
    }
}