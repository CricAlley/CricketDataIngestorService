using System;
using YamlDotNet.Serialization;

namespace PlayerDataGenerator.YamlParser
{
    public class Meta 
    {
        [YamlMember(Alias = "data_version")]
        public decimal DataVersion { get; set; }

        public DateTime Created { get; set; }

        public int Revision { get; set; }
    }
}