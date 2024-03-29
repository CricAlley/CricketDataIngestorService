﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PlayerDataGenerator.Data
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string PlayingRole { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string BattingStyle { get; set; }
        public string BowlingStyle { get; set; }
        public int CricInfoId { get; set; }
        public bool IsActive { get; set; }
        public string CricsheetName { get; set; }
        public string Identifier { get; set; }
    }
}