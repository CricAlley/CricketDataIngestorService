using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using YamlDotNet.RepresentationModel;

namespace CricketDataIngester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

            var players = new HashSet<string>();
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

                    //var list = match.GetPlayers();

                    //foreach (var item in list)
                    //{
                    //    var oldValue = " (sub)";

                    //    if (item.Contains(oldValue))
                    //    {
                    //       var str = item.Replace(oldValue, string.Empty);

                    //       if (!players.Contains(str))
                    //       {
                    //           players.Add(str);
                    //       }
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

            var xmlDocumentCreator = new XmlDocumentCreator();
            var xDocument = xmlDocumentCreator.CreateXML(players.ToList());
            xDocument.Save(@"D:/Players.xml");


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
