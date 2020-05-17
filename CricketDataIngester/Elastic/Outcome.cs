﻿using CricketDataIngester.YamlParser;

namespace CricketDataIngester.Elastic
{
    public class Outcome
    {
        public string Winner { get; set; }

        public By By { get; set; }

        public string BowloutWinner { get; set; }

        public string EliminatorWinner { get; set; }

        public string Method { get; set; }

        public string Result { get; set; }
    }
}