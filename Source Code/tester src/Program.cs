/*
 * 
 * Thusitharan Paramsothy, 500275858
 * Warren Pinto, 500396119
 * 
 * TEST CLASS
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS842_InformationRetrivalAssignment2;
using System.Diagnostics;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    class Program
    {
      //  private static Query query = new Query();

        [STAThread]
        static void Main(string[] args)
        {

       //     Console.WriteLine("Retrieving data..... (De-serializing)");
       //     query.deserializeData();
      //      string term = "";
       //     Console.WriteLine("Include STEMMING? yes or no: ");
      //      string stem = Console.ReadLine();

      //      query.setStem(stem);
     //       Console.WriteLine("Include STOP WORDS? yes or no: ");
     //       string stopw = Console.ReadLine();

       //     query.setStopWord(stopw);

           Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            /*
                        do
                        {

                            Console.WriteLine("Enter a term or type ZZEND to quit:");
                            try
                            {
                                term = Console.ReadLine();
                                Stopwatch stopWatch = new Stopwatch();
                                stopWatch.Start();
                                //Thread.Sleep(1000);
                                if (!term.Equals("ZZEND"))
                                    System.Console.WriteLine("\n\nRetrieving...\n\n");

                                query.parseQuery(term);
                                //System.Console.WriteLine(query.retrieveTermInfo(term.ToLower()));
                                //query.outputDocumentTable("2184");
                                stopWatch.Stop();
                                TimeSpan ts = stopWatch.Elapsed;


                                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                    ts.Hours, ts.Minutes, ts.Seconds,
                                    ts.Milliseconds / 10);
                                Console.WriteLine("Term Retrieval Time: " + elapsedTime);
                            }
                            catch
                            {
                                if (!term.Equals("ZZEND"))
                                    System.Console.WriteLine("Cannot find term");
                            }
                        } while (!term.Equals("ZZEND"));
   
                    }

              */

            /*
             * 
             * 
             */

        }
    }
}
