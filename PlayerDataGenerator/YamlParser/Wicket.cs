using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace PlayerDataGenerator.YamlParser
{
    public class Wicket
    {
        public int Count => 1;

        [YamlMember(Alias = "player_out")]
        public string PlayerOut { get; set; }
        public List<string> Fielders { get; set; }
        public string Kind { get; set; }

        [YamlIgnore]
        public int CaughtCount
        {
            get => Kind.Equals("caught", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int BowledCount
        {
            get => Kind.Equals("bowled", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int RunOutCount
        {
            get => Kind.Equals("run out", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int LBWCount
        {
            get => Kind.Equals("lbw", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int StumpedCount
        {
            get => Kind.Equals("stumped", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int CaughtAndBowledCount
        {
            get => Kind.Equals("caught and bowled", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int RetiredHurtCount
        {
            get => Kind.Equals("retired hurt", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int HitWicketCount
        {
            get => Kind.Equals("hit wicket", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int ObstructingTheFieldCount
        {
            get => Kind.Equals("obstructing the field", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }
    }
}