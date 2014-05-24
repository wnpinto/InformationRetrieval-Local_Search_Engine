using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace ConsoleApplication1
{
    public partial class Form1 : Form
    {
        Query query = new Query();
        static string path = Directory.GetCurrentDirectory();
        //System.IO.StreamWriter queryFile = new System.IO.StreamWriter(path + "\\query.txt");
        string queryFilePath;
        string line = "";
        string qline = "";
        bool readAbstract = false;

        public Form1()
        {
            InitializeComponent();
            outputMsg.Text = "De-serializing";
            query.deserializeData();
            outputMsg.Text = "Serialized!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {

            if (yStemming.Checked)
                query.setStem("yes");
            else
                query.setStem("no");

            if (yStopWords.Checked)
                query.setStopWord("yes");
            else
                query.setStopWord("no");

            query.loadStopWord();


            outputMsg.Text = "Retrieving...\n";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            query.parseQuery(searchText.Text);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;


            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            outputMsg.Text = "RETRIEVED! \n";
            outputText.Text = "Total Number of Documents Retrieved: " + query.getOutputText() + "\n TIME: " + elapsedTime;
        }

        private void evalButton_Click(object sender, EventArgs e)
        {
            if (yStemming.Checked)
                query.setStem("yes");
            else
                query.setStem("no");

            if (yStopWords.Checked)
                query.setStopWord("yes");
            else
                query.setStopWord("no");

            query.loadStopWord();


            string path = Directory.GetCurrentDirectory();
            // Stream fileStream = File.OpenRead(path + "\\dataObj");
            queryFilePath = ""+path + "\\query.txt";
            StreamReader reader = new StreamReader(path + "\\query.text");

            String docAbstractpattern = @"\.W";
            Regex rgxDocAbstract = new Regex(docAbstractpattern, RegexOptions.IgnoreCase);
            String docIDpattern = @"\.I\s[0-9]*";
            Regex rgxDocID = new Regex(docIDpattern, RegexOptions.IgnoreCase);
            String docNpattern = @"\.N";
            Regex rgxN = new Regex(docNpattern, RegexOptions.IgnoreCase);
            string docNum = "";

            try
            {
                do
                {
                    line = reader.ReadLine();
                    if (rgxDocID.IsMatch(line, 0))
                    {
                        var x = line.Split(' ');
                        docNum = x.ElementAt<string>(1); //sets docIDx = document # in collection
                        outputText.Text = "Doc #: " + docNum;
   
                        readAbstract = false;
                    }

                    else if (rgxDocAbstract.IsMatch(line, 0))
                    {
                        readAbstract = true;
                    }

                    else if (rgxN.IsMatch(line, 0))
                    {
                        query.parseQuery(qline);
                        outputText.Text = "Total Number of Documents Retrieved: " + query.getOutputText() + "\n";
                        
                        System.Console.WriteLine(qline);
                        System.Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>DOC# {0}<<<<<<<<<<<<<<<<<<<", docNum);
                        readAbstract = false;
                        qline = "";
                        reader.ReadLine();
                        reader.ReadLine();
                    }

                    else {
                        if (readAbstract == true) {
                            qline+=line+" ";
                        }
                    }
                }
                while (reader.Peek() != -1);
            }
            catch
            {
                System.Console.WriteLine("FILE NOT FOUND!");
            }
            finally
            {
                reader.Close();
            }


        }


    }
}
