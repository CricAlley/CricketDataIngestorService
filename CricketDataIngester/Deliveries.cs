using YamlDotNet.Serialization;

namespace CricketDataIngester
{
    public class Deliveries
    {
        public string Batsman { get; set; }
        public string Bowler { get; set; }

        [YamlMember(Alias = "non_striker", ApplyNamingConventions = false)]
        public string NonStriker { get; set; }

        public Runs Runs { get; set; }

        public Extras Extras { get; set; } 
        
        public Wicket Wicket { get; set; }

        public Replacements Replacements { get; set; }

    }
}