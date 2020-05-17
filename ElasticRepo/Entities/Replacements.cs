using System.Collections.Generic;

namespace ElasticRepo.Entities
{
    public class Replacements
    {
        public List<ReplacementRole> Role { get; set; }
        public List<ReplacementMatch> Match { get; set; }
    }
}