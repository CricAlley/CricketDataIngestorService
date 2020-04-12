using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using CricketDataIngester.Data;

namespace CricketDataIngester
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Player> _players;
        public Form1()
        {
            InitializeComponent();
            _players = new Dictionary<string, Player>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbxFolderPath.Text = @"D:\Projects\Data";
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

            DirectoryInfo d = new DirectoryInfo(text);

            var yamlParser = new YamlParser();

            var directoryInfos = d.GetDirectories();

            //var players = new HashSet<string>();
            foreach (var directoryInfo in directoryInfos)
            {
                lblDirectory.Text = directoryInfo.FullName;

                var files = directoryInfo.GetFiles("*.yaml");

                progressBar1.Maximum = files.Length;
                progressBar1.Step = 1;

                foreach (var file in files)
                {
                    lblFilenameValue.Text = file.Name;

                    var match = yamlParser.Parse(directoryInfo.FullName + "\\" + file.Name);
                    
                    var players = match.GetPlayers();

                    foreach (var player in players)
                    {
                        var dbPlayer = GetPlayer(player);
                    }


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

                    progressBar1.PerformStep();

                }
            }

            //var xmlDocumentCreator = new XmlDocumentCreator();
            //var xDocument = xmlDocumentCreator.CreateXML(players.ToList());
            //xDocument.Save(@"D:/Players.xml");


        }

        private Player GetPlayer(string player)
        {
            if (_players.ContainsKey(player))
            {
                return _players[player];
            }

            Player foundPlayer = null;

            var names = player.Split(' ');

            if(names.Length>2)
                throw new Exception();

            var lastName = names.Last();
            var firstName = names.First();
            var initials = firstName.Split();

            using (var context = new PlayerContext())
            {
                var dbPlayers = context.Players.Where(player1 => player1.FullName.ToUpper().Contains(lastName.ToUpper()));

                foreach (var dbPlayer in dbPlayers)
                {
                    var dbNames = dbPlayer.FullName.Split(' ');
                    var isfound = true;
                    if (dbNames.Last().ToUpper() == lastName.ToUpper())
                    {
                        for (var i = 0; i < initials.Length; i++)
                        {
                            if (dbNames[i][0] != initials[i][0])
                            {
                                isfound = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        isfound = false;
                    }

                    if (isfound)
                    {
                        _players.Add(player, dbPlayer);
                        foundPlayer = dbPlayer;
                    }
                }

                if (foundPlayer == null)
                {
                    throw new Exception();
                }

                return foundPlayer;
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {

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
