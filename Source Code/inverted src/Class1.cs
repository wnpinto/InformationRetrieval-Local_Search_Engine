/*
 * 
 * Thusitharan Paramsothy, 500275858
 * Warren Pinto, 500396119
 * 
 * InvertRunner -> Runner class for invertex index
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace CPS842_InformationRetrivalAssignment2
{
    public class InvertRunner
    {
        public static void Main() {
            Parser p;
            Console.WriteLine("Include STOP WORDS? yes or no: ");
            string stopWord = Console.ReadLine();
            Console.WriteLine("Include STEMMING? yes or no: ");
            string stem = Console.ReadLine();

            bool stemming = false;
            bool stopW = false;

            if (stopWord.Equals("yes"))
            {
                stopW = true;
            }
            else
            {
                stopW = false;
            }
            if (stem.Equals("yes"))
                stemming = true;
            else 
                stemming = false;
            string path = Directory.GetCurrentDirectory();
           // Stream fileStream = File.OpenRead(path + "\\dataObj");
         p = new Parser(path+"\\cacm.all", 1, stopW, stemming);
        // p.extractTextHTML();
       //  p.extractTextPDF();
         p.retrieveFiles();

              //  p.retrieveData();
                System.Console.WriteLine("\n\nSerializing....\n\n");
           
                p.serializeData();
            //SortedDictionary<string, ArrayList> list = p.getPostingsDictionary();
            string term = "";
            System.Console.WriteLine("\n\nOutputting to files....\n\n");
           
            p.outputPostings();
           p.outputDictionary();
           System.Console.WriteLine("\n\nDONE!\n\n");
        }
    }
}
