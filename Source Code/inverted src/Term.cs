using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/*
 * 
 * Thusitharan Paramsothy, 500275858
 * Warren Pinto, 500396119
 */
using System.Text;
using System.Threading.Tasks;

namespace CPS842_InformationRetrivalAssignment2
{
    [Serializable()]
    public class Term
    {
        private string term = "";
        public float frequency {get; set;}
        public string docLine { get; set; }
        public int docLineNum { get; set; }
        public string docNum { get; set; }
        private ArrayList documentIDList = new ArrayList();
        private ArrayList termLineList = new ArrayList();
        private ArrayList documents = new ArrayList();
        private ArrayList documentsHalf = new ArrayList();
        private ArrayList nextTerm = new ArrayList();
        SortedDictionary<string, DocumentNode> docs = new SortedDictionary<string, DocumentNode>();

        public Term(string term, float frequency) { 
            this.term = term;
            this.frequency = frequency;
        }

     
        public void addDocument(string docID)
        {
            documentIDList.Add(docID);
        }


        public void setTerm(string term){
            this.term = term;
        }

        public string getTerm(){
            return term;
        }

        public ArrayList getDocumentIDList() {
            return documentIDList;
        }

        public void addLine(string line)
        {
            termLineList.Add(line);
        }

        public ArrayList getTermLineList()
        {
            return termLineList;
        }

        public void addDocument(DocumentNode doc)
        {
            documents.Add(doc);
        }

        public void addDocumentHalf(DocumentNode doc)
        {
            docs.Add(doc.documentID, doc);
        }

        public SortedDictionary<string, DocumentNode> getDocumentHalf()
        {
            return docs;
        }

        public ArrayList getDocuments()
        {
            return documents;
        }


        public void addNextTerm(Term term)
        {
            nextTerm.Add(term);
        }

        public string getDocumentsString()
        {
            string s = "";
            foreach (DocumentNode doc in documents) {
                    s+=doc.documentID+" ";
            }

            string[] ss = s.Split(' ');
            ss.Distinct();
            s = "";
            foreach (string x in ss) {
                s += "" + x + " ";
            }

            return s;
        }

      /*  public string retrieveLine()
        {
            string line = term;

            foreach (Term t in nextTerm) {
                line += " " + t.nextTerm;
            
            }
        }
       * */
    }

}
