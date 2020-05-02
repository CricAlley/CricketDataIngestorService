using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CricketDataIngester.YamlParser
{
    public class YamlParser
    {
        public  Match Parse(string fileName)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();
            Match m;
            using (Stream stream = File.OpenRead(fileName))
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    m= deserializer.Deserialize<Match>(reader);
                }
            }

            return m;
        }
    }
}