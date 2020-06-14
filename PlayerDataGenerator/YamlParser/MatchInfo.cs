using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace PlayerDataGenerator.YamlParser
{
    public class MatchInfo
    {
        public string Venue { get; set; }
        public string Competition { get; set; }
        public List<DateTime> Dates { get; set; }
        public string Gender { get; set; }
        public List<string> Teams { get; set; }
        public Outcome Outcome { get; set; }
        public Toss Toss { get; set; }

        [YamlMember(Alias = "player_of_match")]
        public List<string> PlayerOfTheMatch { get; set; }

        [YamlMember(Alias = "match_type")]
        public string MatchType { get; set; }

        [YamlMember(Alias = "match_type_number")]
        public string MatchTypeNumber { get; set; }
        public int Overs { get; set; }
        public string City { get; set; }
        public List<string> Umpires { get; set; }

        [YamlMember(Alias = "neutral_venue")]
        public int IsNeutralVenue { get; set; }

        [YamlMember(Alias = "bowl_out")]
        public List<BowlOutDeliveries> BowlOut { get; set; }

        [YamlMember(Alias = "supersubs")]
        public Dictionary<string, string> SuperSubs { get; set; }
    }
}