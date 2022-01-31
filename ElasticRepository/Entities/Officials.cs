using System.Collections.Generic;

namespace ElasticRepository.Entities
{
    public class Officials
    {
        public List<string> MatchReferees { get; set; } = new List<string>();

        public List<string> ReserveUmpires { get; set; } = new List<string>();

        public List<string> TvUmpires { get; set; } = new List<string>();

        public List<string> Umpires { get; set; } = new List<string>();
    }
}