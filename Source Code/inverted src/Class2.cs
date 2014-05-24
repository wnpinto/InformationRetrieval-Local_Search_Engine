/*
 * 
 * Thusitharan Paramsothy, 500275858
 * Warren Pinto, 500396119
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS842_InformationRetrivalAssignment2
{
    //Class that holds Document information such as document frequency, document id, etc..
   [Serializable()]
    public class DocumentNode
    {
        private string docIDx;
        private int p;
        public string line = "";
        public int lineNum { get; set; }
        public int wordNum { get; set; }
        public double similarity { get; set; }
        public double docScalar { get; set; }
        public double weight { get; set; }
        public double idf { get; set; }
        public double wtf { get; set; }
        public string documentID { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public int frequency { get; set; }
        
        public string word { get; set; }

        public DocumentNode(string documentID, int frequency, string word)
        {
            this.documentID = documentID;
            this.frequency = frequency;
            this.word = word;
        }

        public DocumentNode()
        {
            // TODO: Complete member initialization
        }

        public DocumentNode(string docIDx, int p, int lineNum, int wordNum, string line, string author, string word)
        {
            // TODO: Complete member initialization
            this.documentID = docIDx;
            this.frequency = p;
            this.lineNum = lineNum;
            this.wordNum = wordNum;
            this.line = line;
            this.author = author;
            this.word = word;
        }
    }
}
