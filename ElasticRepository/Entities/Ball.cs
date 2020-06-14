using System;
using System.Globalization;

namespace ElasticRepository.Entities
{
    public class Ball
    {
        private decimal _deliveryKey;
        public string Id { get; set; }
        public  Match Match { get; set; }
        public Inning Inning { get; set; }

        public decimal DeliveryKey  
        {
            get => _deliveryKey;
            set
            {
                _deliveryKey = value;
                Over = (int) Math.Truncate(value);
                BallNumber = GetFrac2Digits(value);
            }
        }

        public string OverId { get; set; }
        public int Over { get; set; }
        public int BallNumber { get; set; }
        public Player Batsman { get; set; }
        public Player Bowler { get; set; }
        public Player NonStriker { get; set; }
        public Runs Runs { get; set; }
        public Extras Extras { get; set; }
        public Wicket Wicket { get; set; }
        public Replacements Replacements { get; set; }
        public int GetFrac2Digits(decimal d)
        {
            var str = d.ToString("0.0", CultureInfo.InvariantCulture);
            return int.Parse(str.Substring(str.IndexOf('.') + 1));
        }
    }
}
