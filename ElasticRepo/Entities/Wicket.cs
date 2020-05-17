using System.Collections.Generic;

namespace ElasticRepo.Entities
{
    public class Wicket 
    {
        public int Count => 1;

        public string PlayerOut { get; set; }
        public List<string> Fielders { get; set; }
        public string Kind { get; set; }

        public int CaughtCount
        {
            get => Kind.ToLower() == "caught" ? 1 : 0;
            set { }
        }

        public int BowledCount
        {
            get => Kind.ToLower() == "bowled" ? 1 : 0;
            set { }
        }

        public int RunOutCount
        {
            get => Kind.ToLower() == "run out" ? 1 : 0;
            set { }
        }

        public int LBWCount
        {
            get => Kind.ToLower() == "lbw" ? 1 : 0;
            set { }
        }

        public int StumpedCount
        {
            get => Kind.ToLower() == "stumped" ? 1 : 0;
            set { }
        }

        public int CaughtAndBowledCount
        {
            get => Kind.ToLower() == "caught and bowled" ? 1 : 0;
            set { }
        }

        public int RetiredHurtCount
        {
            get => Kind.ToLower() == "retired hurt" ? 1 : 0;
            set { }
        }

        public int HitWicketCount
        {
            get => Kind.ToLower() == "hit wicket" ? 1 : 0;
            set { }
        }

        public int ObstructingTheFieldCount
        {
            get => Kind.ToLower() == "obstructing the field" ? 1 : 0;
            set { }
        }
    }
}