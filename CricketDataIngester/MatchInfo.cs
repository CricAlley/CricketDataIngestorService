using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace CricketDataIngester
{
    public class MatchInfo
    {
        public string Venue { get; set; }
        public string Competition { get; set; }
        public List<DateTime> Dates { get; set; }
        public string Gender { get; set; }
        public List<string> Teams { get; set; }
        public Outcome Outcome { get; set; }

        public string Result { get; set; }
        public Toss Toss { get; set; }

        [YamlMember(Alias = "player_of_match", ApplyNamingConventions = false)]
        public List<string> PlayerOfTheMatch { get; set; }

        [YamlMember(Alias = "match_type", ApplyNamingConventions = false)]
        public string MatchType { get; set; }
        public int Overs { get; set; }
        public string City { get; set; }
        public List<string> Umpires { get; set; }

        [YamlMember(Alias = "neutral_venue", ApplyNamingConventions = false)]
        public int IsNeutralVenue { get; set; }

        [YamlMember(Alias = "bowl_out", ApplyNamingConventions = false)]
        public List<BowlOutDeliveries> BowlOut { get; set; }

        [YamlMember(Alias = "supersubs", ApplyNamingConventions = false)]
        public Dictionary<string, string> SuperSubs { get; set; }


    }
}