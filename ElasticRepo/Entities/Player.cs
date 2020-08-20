using System;

namespace ElasticRepo.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public string PlayingRole { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BattingStyle { get; set; }
        public string BowlingStyle { get; set; }
        public int CricInfoId { get; set; }
        public string CricsheetName { get; set; }
        public string Team { get; set; }
    }
}