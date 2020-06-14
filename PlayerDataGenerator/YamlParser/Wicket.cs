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
            get => Kind.ToLower() == "caught" ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int BowledCount
        {
            get => Kind.ToLower() == "bowled" ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int RunOutCount
        {
            get => Kind.ToLower() == "run out" ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int LBWCount
        {
            get => Kind.ToLower() == "lbw" ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int StumpedCount
        {
            get => Kind.ToLower() == "stumped" ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int CaughtAndBowledCount
        {
            get => Kind.ToLower() == "caught and bowled" ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int RetiredHurtCount
        {
            get => Kind.ToLower() == "retired hurt" ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int HitWicketCount
        {
            get => Kind.ToLower() == "hit wicket" ? 1 : 0;
            set { }
        }

        [YamlIgnore]
        public int ObstructingTheFieldCount
        {
            get => Kind.ToLower() == "obstructing the field" ? 1 : 0;
            set { }
        }
    }
}