using System;
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
            get => Kind.Equals("caught", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        public int BowledCount
        {
            get => Kind.Equals("bowled", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        public int RunOutCount
        {
            get => Kind.Equals("run out", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        public int LBWCount
        {
            get => Kind.Equals("lbw", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        public int StumpedCount
        {
            get => Kind.Equals("stumped", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        public int CaughtAndBowledCount
        {
            get => Kind.Equals("caught and bowled", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        public int RetiredHurtCount
        {
            get => Kind.Equals("retired hurt", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        public int HitWicketCount
        {
            get => Kind.Equals("hit wicket", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }

        public int ObstructingTheFieldCount
        {
            get => Kind.Equals("obstructing the field", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            set { }
        }
    }
}