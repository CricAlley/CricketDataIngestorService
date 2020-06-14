using System;
using YamlDotNet.Serialization;

namespace CricketDataIngester.YamlParser
{
    public class Meta 
    {
        public decimal DataVersion { get; set; }

        public DateTime Created { get; set; }

        public int Revision { get; set; }
    }
}