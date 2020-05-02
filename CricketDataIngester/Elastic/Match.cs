using System;
using System.Collections.Generic;
using CricketDataIngester.YamlParser;

namespace CricketDataIngester.Elastic
{
    public class Match
    {
        public string MatchId { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string City { get; set; }
        public string MatchType { get; set; }
        public string Competition { get; set; }
        public int Overs { get; set; }
        public string Gender { get; set; }
        public List<string> Teams { get; set; }
        public Outcome Outcome { get; set; }
        public Toss Toss { get; set; }
        public List<string> PlayerOfTheMatch { get; set; }
        public List<string> Umpires { get; set; }
        public int IsNeutralVenue { get; set; }
        public List<BowlOutDeliveries> BowlOut { get; set; }
        public Dictionary<string, string> SuperSubs { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
    }
}