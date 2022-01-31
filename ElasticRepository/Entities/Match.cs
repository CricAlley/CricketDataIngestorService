using System;
using System.Collections.Generic;

namespace ElasticRepository.Entities
{
    public class Match
    {           
        public string MatchType { get; set; }
        public Event Event { get; set; }
        public int Overs { get; set; }
        public string Gender { get; set; }
        public List<string> Teams { get; set; }
        public Outcome Outcome { get; set; }
        public Toss Toss { get; set; }
        public List<string> PlayerOfTheMatch { get; set; }
        public Officials Officials { get; set; }
        public List<BowlOut> BowlOut { get; set; }
        public Dictionary<string, string> SuperSubs { get; set; }
        public string Season { get; set; }
        public string TeamType { get; set; }
        public int MatchTypeNumber { get; set; }
    }
}