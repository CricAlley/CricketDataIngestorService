using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using AutoMapper;
using CricketDataIngester.Data;
using CricketDataIngester.YamlParser;
using ElasticRepo;
using ElasticRepo.Entities;
using ElasticRepo.Indices;
using Inning = ElasticRepo.Entities.Inning;
using Match = ElasticRepo.Entities.Match;
using Player = CricketDataIngester.Data.Player;

namespace CricketDataIngester
{
    public partial class Form1 : Form
    {
        private readonly List<Tuple<string, int>> _ambigousPlayers;
        private IMapper _mapper;
        private readonly Dictionary<string, Player> _players;

        public Form1()
        {
            InitializeComponent();
            _players = new Dictionary<string, Player>();
            _ambigousPlayers = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("A Singh", 26789),
                new Tuple<string, int>("Jaskaran Singh", 376102),
                new Tuple<string, int>("R Sharma", 272994),
                new Tuple<string, int>("N Saini", 274785),
                new Tuple<string, int>("R Shukla", 390547)
            };


            using (var context = new PlayerContext())
            {
                foreach (var plr in _ambigousPlayers)
                {
                    var player = context.Players.First(p => p.CricInfoId == plr.Item2);
                    player.CricsheetName = plr.Item1;
                    _players.Add(plr.Item1, player);
                }

                context.SaveChanges();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbxFolderPath.Text = @"D:\Elastic\11-04-2020";

            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

            _mapper = mapperConfiguration.CreateMapper();
        }

        private void FolderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }

        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            tbxFolderPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            var text = tbxFolderPath.Text;


            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(@"Please select a folder!");
                return;
            }

            var d = new DirectoryInfo(text);


            var directoryInfos = d.GetDirectories().First();

            lblDirectory.Text = directoryInfos.FullName;

            var files = directoryInfos.GetFiles("*.yaml");


            progressBar1.Maximum = files.Length;
            progressBar1.Step = 1;

            backgroundWorker.RunWorkerAsync(files);
            btnStart.Enabled = false;


            //var xmlDocumentCreator = new XmlDocumentCreator();
            //var xDocument = xmlDocumentCreator.CreateXML(players.ToList());
            //xDocument.Save(@"D:/Players.xml");
        }

        private void IngestData(FileInfo[] files)
        {
            var yamlParser = new YamlParser.YamlParser();

            foreach (var file in files)
            {
                var match = yamlParser.Parse(file.FullName);

                var players = match.GetPlayers();

                // Update Cricsheet name in DB
                foreach (var player in players) UpdatePlayer(player);

                var balls = new List<Ball>();

                var elasticClient = new ElasticClientProvider().GetElasticClient();

                var matchId = GetMatchId(match.MatchInfo, file);


                foreach (var inning in match.Innings)
                {
                    var inningValue = inning.Values.First();
                    foreach (var delivery in inningValue.Deliveries)
                    {
                        var deliveryValue = delivery.Values.First();
                        var ball = _mapper.Map<Ball>(deliveryValue);
                        var bowler = _players[deliveryValue.Bowler];
                        ball.Bowler = _mapper.Map<ElasticRepo.Entities.Player>(bowler);

                        var batsman = _players[deliveryValue.Batsman];
                        ball.Batsman = _mapper.Map<ElasticRepo.Entities.Player>(batsman);

                        var nonStriker = _players[deliveryValue.NonStriker];
                        ball.NonStriker = _mapper.Map<ElasticRepo.Entities.Player>(nonStriker);

                        ball.DeliveryKey = delivery.Keys.First();

                        var innings = _mapper.Map<Inning>(inningValue);
                        var matchInfo = match.MatchInfo;

                        innings.Innings = inning.First().Key;
                        innings.BowlingTeam = matchInfo.Teams.First(s => s != innings.BattingTeam);
                        ball.Inning = innings;

                        var ballMatch = _mapper.Map<Match>(matchInfo);
                        ball.Match = ballMatch;
                        ball.Match.MatchId = matchId;
                        var inningsId = matchId + innings.Innings;
                        ball.Inning.InningsId = inningsId;
                        ball.OverId = inningsId + ball.Over;

                        balls.Add(ball);
                    }
                }

                var bulkIndexResponse = elasticClient.Bulk(b => b
                    .Index("iplballs")
                    .IndexMany(balls));


                //ListPlayers(players);
                backgroundWorker.ReportProgress(1, file.FullName);
            }
        }

        private static string GetMatchId(MatchInfo matchInfo, FileInfo file)
        {
            return matchInfo.Dates.First().ToLongDateString() + matchInfo.City +
                   string.Join("", matchInfo.Teams.Select(s => s.Replace(" ", ""))) +
                   file.Name;
        }

        private static void ListPlayers(List<string> players)
        {
            //foreach (var item in list)
            //{
            //    var oldValue = " (sub)";

            //    if (item.Contains(oldValue))
            //    {
            //        var str = item.Replace(oldValue, string.Empty);

            //        if (!players.Contains(str))
            //        {
            //            players.Add(str);
            //        }
            //    }
            //    else
            //    {
            //        if (!players.Contains(item))
            //        {
            //            players.Add(item);
            //        }
            //    }
            //}
        }

        private void UpdatePlayer(string player)
        {
            player = player.Replace(" (sub)", "");
            if (_players.ContainsKey(player)) return;

            if (_ambigousPlayers.Any(p => p.Item1 == player)) return;

            Player foundPlayer = null;

            var names = player.Split(' ');


            var lastName = names.Last();
            var firstName = names.First();
            var isInitials = firstName.ToCharArray().All(c => c == char.ToUpper(c));

            var middleName = string.Empty;
            if (names.Length > 2) middleName = names.Skip(1).First();

            using (var context = new PlayerContext())
            {
                if (isInitials)
                {
                    var dbPlayers = context.Players.Where(player1 =>
                        player1.FullName.ToUpper().Contains(lastName.ToUpper()) && player1.IsActive);

                    foreach (var dbPlayer in dbPlayers)
                    {
                        var dbNames = dbPlayer.FullName.Split(' ');
                        var dbLastName = dbNames.Last();
                        var dbFirstName = dbNames.First();
                        var isDbNameInitials = dbFirstName.ToCharArray().All(c => c == char.ToUpper(c));

                        var isfound = false;
                        if (dbLastName.ToUpper() == lastName.ToUpper())
                        {
                            isfound = true;
                            var j = 0;
                            for (var i = 0; i < firstName.Length; i++, j++)
                                if (dbNames[i][0] != firstName[i])
                                {
                                    isfound = false;
                                    break;
                                }

                            if (!string.IsNullOrWhiteSpace(middleName) && dbNames[j].ToUpper() != middleName.ToUpper())
                                isfound = false;

                            if (isDbNameInitials) isfound = dbFirstName == firstName;
                        }

                        if (isfound)
                        {
                            _players.Add(player, dbPlayer);
                            foundPlayer = dbPlayer;

                            dbPlayer.CricsheetName = player;
                        }
                    }

                    if (foundPlayer == null) throw new Exception();
                }
                else
                {
                    var dbPlayers = context.Players.Where(player1 =>
                        player1.FullName.ToUpper().Contains(firstName.ToUpper()) && player1.IsActive);

                    foreach (var dbPlayer in dbPlayers)
                    {
                        var dbNames = dbPlayer.Name.Split(' ');
                        var dbFirstName = dbNames.First();
                        var dbLastName = dbNames.Last();
                        var dbMiddleName = string.Empty;
                        if (dbNames.Length > 2) dbMiddleName = dbNames.Skip(1).FirstOrDefault();

                        if (firstName.ToUpper() == dbFirstName.ToUpper() &&
                            lastName.ToUpper() == dbLastName.ToUpper() &&
                            middleName?.ToUpper() == dbMiddleName?.ToUpper())
                        {
                            _players.Add(player, dbPlayer);
                            foundPlayer = dbPlayer;
                            dbPlayer.CricsheetName = player;
                        }
                    }

                    if (foundPlayer == null) throw new Exception();
                }

                context.SaveChanges();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
        }

        private void BtnCreateIndex_Click(object sender, EventArgs e)
        {
            var createIndexResponse = new IndexCreator().CreateIplIndex();

            MessageBox.Show($"Index created : {createIndexResponse.Acknowledged.ToString()}");
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Enabled = true;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.PerformStep();

            lblFilenameValue.Text = e.UserState.ToString();

        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var files = e.Argument as FileInfo[];
            IngestData(files);
        }
    }

    public class XmlDocumentCreator
    {
        public XDocument CreateXML(List<string> players)
        {
            var xDocument = new XDocument();

            var root = new XElement("Players");

            xDocument.Add(root);

            foreach (var player in players)
            {
                var xElement = new XElement("Player", player);

                root.Add(xElement);
            }

            return xDocument;
        }
    }
}