using YamlDotNet.Serialization;

namespace PlayerDataGenerator.YamlParser
{
    public class Deliveries
    {
        public string Batsman { get; set; }
        public string Bowler { get; set; }

        [YamlMember(Alias = "non_striker")]
        public string NonStriker { get; set; }

        public Runs Runs { get; set; }

        public Extras Extras { get; set; } 
        
        public Wicket Wicket { get; set; }

        public Replacements Replacements { get; set; }

    }
}