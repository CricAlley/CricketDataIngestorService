using System;
using YamlDotNet.Serialization;

namespace CricketDataIngester
{
    public class Meta 
    {
        [YamlMember(Alias = "data_version", ApplyNamingConventions = false)]
        public decimal DataVersion { get; set; }

        public DateTime Created { get; set; }

        public int Revision { get; set; }
    }
}