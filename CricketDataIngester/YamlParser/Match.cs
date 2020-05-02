using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace CricketDataIngester.YamlParser
{
    public class Match    
    {

        public Meta Meta { get; set; }

        [YamlMember(Alias = "info", ApplyNamingConventions = false)]
        public MatchInfo MatchInfo { get; set; }

        public List<Dictionary<string, Inning>> Innings { get; set; }

        public List<string> GetPlayers()
        {
            var players = new HashSet<string>();

            if (MatchInfo.BowlOut != null)
            {
                foreach (var bowlOutDeliveriese in MatchInfo.BowlOut)
                {
                    var bowler = bowlOutDeliveriese.Bowler;
                    if (!players.Contains(bowler))
                    {
                        players.Add(bowler);
                    }
                }
            }

            foreach (var inning in Innings)
            {
                var inningItem2 = inning.Values.First();

                if (inningItem2.AbsentHurt != null)
                {
                    foreach (var absentHurt in inningItem2.AbsentHurt)
                    {
                        if (!players.Contains(absentHurt))
                        {
                            players.Add(absentHurt);
                        }
                    }
                }

                foreach (var delivery in inningItem2.Deliveries)
                {
                    var deliveryItem2 = delivery.Values.First();
                    if (!players.Contains(deliveryItem2.Bowler))
                    {
                        players.Add(deliveryItem2.Bowler);
                    }

                    if (!players.Contains(deliveryItem2.Batsman))
                    {
                        players.Add(deliveryItem2.Batsman);
                    }

                    if (!players.Contains(deliveryItem2.NonStriker))
                    {
                        players.Add(deliveryItem2.NonStriker);
                    }

                    if (deliveryItem2.Wicket != null)
                    {
                        if (!players.Contains(deliveryItem2.Wicket.PlayerOut))
                        {
                            players.Add(deliveryItem2.Wicket.PlayerOut);
                        }

                        if (deliveryItem2.Wicket.Fielders != null)
                        {
                            foreach (var fielder in deliveryItem2.Wicket.Fielders)
                            {
                                if (!players.Contains(fielder))
                                {
                                    players.Add(fielder);
                                }
                            }
                        }
                    }

                    if (deliveryItem2.Replacements?.Role != null)
                    {
                        foreach (var role in deliveryItem2.Replacements.Role)
                        {
                            if (!players.Contains(role.In))
                            {
                                players.Add(role.In);
                            }

                            if (!string.IsNullOrEmpty(role.Out))
                            {
                                if (!players.Contains(role.Out))
                                {
                                    players.Add(role.Out);
                                }
                            }
                        }


                    }

                    if (deliveryItem2.Replacements?.Match != null)
                    {
                        foreach (var match in deliveryItem2.Replacements.Match)
                        {
                            if (!players.Contains(match.In))
                            {
                                players.Add(match.In);
                            }

                            if (!string.IsNullOrEmpty(match.Out))
                            {
                                if (!players.Contains(match.Out))
                                {
                                    players.Add(match.Out);
                                }
                            }
                        }

                    }
                }
            }


            return players.ToList();
        }
       
    }
}