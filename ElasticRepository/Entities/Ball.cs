using System;
using System.Collections.Generic;

namespace ElasticRepository.Entities
{
    public class Ball
    {
        public  Match Match { get; set; }
        public Inning Inning { get; set; }
        public string OverId { get; set; }
        public int OverNumber { get; set; }
        public int BallNumber { get; set; }
        public Player Batsman { get; set; }
        public Player Bowler { get; set; }
        public Player NonStriker { get; set; }
        public Runs Runs { get; set; }
        public Extras Extras { get; set; }
        public List<Wicket> Wickets { get; set; }
        public Replacements Replacements { get; set; }
        public string BowlingTeam { get; set; }
        public string BattingTeam { get; set; }
        public bool IsPowerPlay { get; set; }
        public bool IsSuperOver { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string City { get; set; }
        public string MatchId { get; set; }
        public string InningsId { get; set; }
        public string ReviewBy { get; set; }
        public string ReviewUmpire { get; set; }
        public string ReviewBatter { get; set; }
        public string ReviewDecision { get; set; }
        public bool IsUmpiresCall { get; set; }
    }
}
