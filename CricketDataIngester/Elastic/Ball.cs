using CricketDataIngester.YamlParser;

namespace CricketDataIngester.Elastic
{
    public class Ball
    {
        public  Match Match { get; set; }
        public Inning Inning { get; set; }
        public decimal DeliveryNumber { get; set; }
        public Player Batsman { get; set; }
        public Player Bowler { get; set; }
        public Player NonStriker { get; set; }
        public Runs Runs { get; set; }
        public Extras Extras { get; set; }
        public Wicket Wicket { get; set; }
        public Replacements Replacements { get; set; }
    }
}
