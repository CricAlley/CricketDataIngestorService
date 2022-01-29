using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerDataGenerator
{
    public static class Constants
    {
        public static string GENERAL_SETTINGS = "GeneralSettings";
        public static string FailedPlayerFile = "FailedPlayers.csv";
        public static string IncludedTeams = "IncludedTeams.txt";
        public static string PlayerScript = "PlayerScript.sql";

        public class DBConnections 
        {
            public static string CRICKET_DB = "CricketDB";
        }
    }
}
