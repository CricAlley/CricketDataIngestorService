using Newtonsoft.Json;
using System.IO;

namespace PlayerDataGenerator.JsonParser
{
    public class JsonParser
    {
        public CricketMatch Parse(string fileName)
        {
            CricketMatch m;
            JsonSerializerSettings settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
            using (Stream stream = File.OpenRead(fileName))
            {
                using TextReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();
                m = JsonConvert.DeserializeObject<CricketMatch>(json, settings);
            }

            return m;
        }
    }

}
