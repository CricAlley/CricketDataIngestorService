using YamlDotNet.Serialization;

namespace PlayerDataGenerator.YamlParser
{
    public class Outcome
    {
        public string Winner { get; set; }

        public By By { get; set; }

        [YamlMember(Alias = "bowl_out")]
        public string BowloutWinner { get; set; }

        [YamlMember(Alias = "eliminator")]
        public string EliminatorWinner { get; set; }

        public string Method { get; set; }

        public string Result { get; set; }
    }
}