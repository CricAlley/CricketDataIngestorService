using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace PlayerDataGenerator.YamlParser
{
    public class Inning
    {
        public string Team { get; set; }

        public List<Dictionary<decimal, Deliveries>> Deliveries { get; set; }

        [YamlMember(Alias = "absent_hurt")]
        public List<string> AbsentHurt { get; set; }

        [YamlMember(Alias = "penalty_runs")]
        public PenaltyRuns PenaltyRuns { get; set; }
        [YamlMember(Alias = "declared")]
        public int IsDeclared { get; set; }
    }
}