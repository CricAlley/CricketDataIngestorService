using PlayerDataGenerator.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PlayerDataGenerator
{

    public interface IPlayerScriptGenerator
    {
        void GenerateScript();
    }

    public class PlayerScriptGenerator : IPlayerScriptGenerator
    {
        private readonly CricketContext _playerContext;

        public PlayerScriptGenerator(CricketContext playerContext)
        {
            _playerContext = playerContext;
        }
        public void GenerateScript()
        {
            var players = _playerContext.Players.OrderBy(p=>p.CricInfoId).ToList();

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("IF OBJECT_ID('tempdb..#players') IS NOT NULL");
            stringBuilder.AppendLine("  DROP TABLE #players");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("CREATE TABLE #players");
            stringBuilder.AppendLine("(");
            stringBuilder.AppendLine("  [Id]            INT             IDENTITY(1, 1) NOT NULL,");
            stringBuilder.AppendLine("  [Name]          NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [FullName]      NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [PlayingRole]   NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [DateOfBirth]   DATETIME        NULL,");
            stringBuilder.AppendLine("  [BattingStyle]  NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [BowlingStyle]  NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine("  [CricInfoId]    INT             NOT NULL,");
            stringBuilder.AppendLine("  [IsActive]      BIT             DEFAULT((1)),");
            stringBuilder.AppendLine("  [CricsheetName] NVARCHAR(MAX)   NULL,");
            stringBuilder.AppendLine(");");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(
                "INSERT INTO #players	([Name], [FullName], [PlayingRole], [DateOfBirth], [BattingStyle], [BowlingStyle], [CricInfoId], [IsActive], [CricsheetName])");

            var lastPlayerIndex = players.Count() - 1;

            for (var index = 0; index < players.Count(); index++)
            {
                var player = players[index];

                var canSplitBatch = index % 1000 == 0;
                var cricSheetName = player.CricsheetName == null ? "NULL" : $"'{player.CricsheetName.Replace("'", "''")}'";
                var dateOfBirth = player.DateOfBirth == null ? "NULL" : $"'{player.DateOfBirth}'";
                if (canSplitBatch)
                {

                    stringBuilder.AppendLine(
                        $@"SELECT '{player.Name.Replace("'", "''")}' AS Name, '{player.FullName.Replace("'", "''")}' AS FullName, '{player.PlayingRole}' AS PlayingRole, {dateOfBirth} AS DateofBirth," +
                        $@" '{player.BattingStyle}' AS BattingStyle, '{player.BowlingStyle}' AS BowlingStyle, {player.CricInfoId} AS CricInfoId, 1 AS IsActive, {cricSheetName} AS CricsheetName");

                    stringBuilder.AppendLine("Go");

                    stringBuilder.AppendLine();

                    stringBuilder.AppendLine(
                        "INSERT INTO #players	([Name], [FullName], [PlayingRole], [DateOfBirth], [BattingStyle], [BowlingStyle], [CricInfoId], [IsActive], [CricsheetName])");
                }
                else if (index == lastPlayerIndex)
                {

                    stringBuilder.AppendLine(
                        $@"SELECT '{player.Name.Replace("'", "''")}' AS Name, '{player.FullName.Replace("'", "''")}' AS FullName, '{player.PlayingRole}' AS PlayingRole, {dateOfBirth} AS DateofBirth," +
                        $@" '{player.BattingStyle}' AS BattingStyle, '{player.BowlingStyle}' AS BowlingStyle, {player.CricInfoId} AS CricInfoId, 1 AS IsActive, {cricSheetName} AS CricsheetName");

                }
                else
                {
                    stringBuilder.AppendLine(
                        $@"SELECT '{player.Name.Replace("'", "''")}' AS Name, '{player.FullName.Replace("'", "''")}' AS FullName, '{player.PlayingRole}' AS PlayingRole, {dateOfBirth}  AS DateofBirth," +
                        $@" '{player.BattingStyle}' AS BattingStyle, '{player.BowlingStyle}' AS BowlingStyle, {player.CricInfoId} AS CricInfoId, 1 AS IsActive, {cricSheetName} AS CricsheetName UNION ALL");
                }
            }

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("BEGIN TRY");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("  BEGIN TRANSACTION");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("      MERGE  dbo.players AS TARGET");
            stringBuilder.AppendLine("          USING #players AS SOURCE");
            stringBuilder.AppendLine("          ON(TARGET.CricInfoId = SOURCE.CricInfoId)");
            stringBuilder.AppendLine("              WHEN MATCHED");
            stringBuilder.AppendLine("              THEN");
            stringBuilder.AppendLine("                  UPDATE");
            stringBuilder.AppendLine("                  SET TARGET.PlayingRole = SOURCE.PlayingRole");
            stringBuilder.AppendLine("                      , TARGET.CricSheetName = SOURCE.CricSheetName");
            stringBuilder.AppendLine("                      , TARGET.FullName = SOURCE.FullName");
            stringBuilder.AppendLine("                      , TARGET.DateOfBirth = SOURCE.DateOfBirth");
            stringBuilder.AppendLine("                      , TARGET.BattingStyle = SOURCE.BattingStyle");
            stringBuilder.AppendLine("                      , TARGET.BowlingStyle = SOURCE.BowlingStyle");
            stringBuilder.AppendLine("                      , TARGET.CricInfoId = SOURCE.CricInfoId");
            stringBuilder.AppendLine("                      , TARGET.IsActive = SOURCE.IsActive");
            stringBuilder.AppendLine("              WHEN NOT MATCHED BY TARGET");
            stringBuilder.AppendLine("              THEN");
            stringBuilder.AppendLine("                  INSERT(Name");
            stringBuilder.AppendLine("                        , FullName");
            stringBuilder.AppendLine("                        , PlayingRole");
            stringBuilder.AppendLine("                        , DateOfBirth");
            stringBuilder.AppendLine("                        , BattingStyle");
            stringBuilder.AppendLine("                        , BowlingStyle");
            stringBuilder.AppendLine("                        , CricInfoId");
            stringBuilder.AppendLine("                        , IsActive");
            stringBuilder.AppendLine("                        , CricsheetName)");
            stringBuilder.AppendLine("                  VALUES(SOURCE.Name");
            stringBuilder.AppendLine("                        , SOURCE.FullName");
            stringBuilder.AppendLine("                        , SOURCE.PlayingRole");
            stringBuilder.AppendLine("                        , SOURCE.DateOfBirth");
            stringBuilder.AppendLine("                        , SOURCE.BattingStyle");
            stringBuilder.AppendLine("                        , SOURCE.BowlingStyle");
            stringBuilder.AppendLine("                        , SOURCE.CricInfoId");
            stringBuilder.AppendLine("                        , SOURCE.IsActive");
            stringBuilder.AppendLine("                        , SOURCE.CricsheetName);");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("  COMMIT TRANSACTION");
            stringBuilder.AppendLine("PRINT 'MERGE dbo.Player - Done'");
            stringBuilder.AppendLine("END TRY");
            stringBuilder.AppendLine("BEGIN CATCH");
            stringBuilder.AppendLine("      ROLLBACK TRANSACTION;");
            stringBuilder.AppendLine("      THROW");
            stringBuilder.AppendLine("END CATCH");

            File.WriteAllText("PlayersData.sql", stringBuilder.ToString());

            Console.WriteLine("Player Data script is generated.");
        }
    }
}
