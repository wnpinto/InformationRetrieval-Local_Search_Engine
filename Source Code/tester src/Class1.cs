/*
 * 
 * Thusitharan Paramsothy, 500275858
 * Warren Pinto, 500396119
 * 
 * InvertRunner -> Runner class for invertex index
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CPS842_InformationRetrivalAssignment2;
using System.Collections;
using System.Text.RegularExpressions;


namespace ConsoleApplication1
{
    class Query
    {
        SortedDictionary<string, Term> termDict = new SortedDictionary<string, Term>();
        SortedDictionary<string, DocumentNode> docList = new SortedDictionary<string, DocumentNode>();
        // SortedDictionary<string, ArrayList> query = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, double> doc_norm = new SortedDictionary<string, double>();
        SortedDictionary<string, double> q_norm = new SortedDictionary<string, double>();
        SortedDictionary<string, double> doc_nw = new SortedDictionary<string, double>();
        ArrayList q_nw = new ArrayList();
        SortedDictionary<string, double> doc_sim = new SortedDictionary<string, double>();
        SortedDictionary<string, double> queryTerms = new SortedDictionary<string, double>();
        SortedDictionary<string, double> queryTerms2 = new SortedDictionary<string, double>();
        SortedDictionary<string, SortedDictionary<string, DocumentNode>> term_Docs = new SortedDictionary<string, SortedDictionary<string, DocumentNode>>();
        static string path = Directory.GetCurrentDirectory();
        System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\TABLE.txt");
        System.IO.StreamWriter file2 = new System.IO.StreamWriter(path + "\\TABLE_NORM.txt");
        System.IO.StreamWriter file3 = new System.IO.StreamWriter(path + "\\docSim.txt");
        //---------------

        SortedDictionary<string, double> query = new SortedDictionary<string, double>();
        SortedDictionary<string, double> allTerms = new SortedDictionary<string, double>();
        SortedDictionary<string, ArrayList> allTermsTable = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, double> allTermsDoc = new SortedDictionary<string, double>();
        SortedDictionary<string, SortedDictionary<string, DocumentNode>> relevantDocs = new SortedDictionary<string, SortedDictionary<string, DocumentNode>>();
        SortedDictionary<string, DocumentNode> relevantDocsList = new SortedDictionary<string, DocumentNode>();
        SortedDictionary<string, SortedDictionary<string, ArrayList>> relevantDocsTable = new SortedDictionary<string, SortedDictionary<string, ArrayList>>();
        SortedDictionary<string, ArrayList> DocsWeights = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, ArrayList> DocsNWeights = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, ArrayList> QWeights = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, ArrayList> QNWeights = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, ArrayList> qWeights = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, ArrayList> qNWeights = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, ArrayList> qidf = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, SortedDictionary<string, double>> qDocs = new SortedDictionary<string, SortedDictionary<string, double>>();
        SortedDictionary<string, double> docSim = new SortedDictionary<string, double>();
        ArrayList stopWordList = new ArrayList();
        string outputText { get; set;}

        private static string isStem { get; set; }
        private static string isStopWord { get; set; }

        double qScalar = 0;

        public Query()
        {


        }

        public void deserializeData()
        {
            string p = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            p = Directory.GetParent(p).ToString();
            p = Directory.GetParent(p).ToString();

            Stream fileStream = File.OpenRead(p + "\\ConsoleApplication1\\bin\\Debug\\dataObj");
            Stream fileStream2 = File.OpenRead(p + "\\ConsoleApplication1\\bin\\Debug\\dataObj2");
            BinaryFormatter deSerializer = new BinaryFormatter();
            termDict = (SortedDictionary<string, Term>)deSerializer.Deserialize(fileStream);
            docList = (SortedDictionary<string, DocumentNode>)deSerializer.Deserialize(fileStream2);

        }

        public void setStem(string stem)
        {
            isStem = stem;
        }

        public void setStopWord(string sw)
        {
            isStopWord = sw;
        }

        public string retrieveTermInfo(string keyword)
        {
            string info = "";

            keyword = keyword.ToLower();
            //  if (keyword.Length > 3)
            //    keyword = porter.checkEnd(keyword.ToCharArray());

            info += "\nTERM:  " + keyword;
            info += "\nTerm Frequency: " + termDict[keyword].frequency;
            Stem stem = new Stem();
            int count = 0;

            if (isStem.Equals("yes"))
            {

                keyword = stem.step1(keyword);
            }

            foreach (KeyValuePair<string, Term> entry in termDict)
            {
                string word = entry.Key;

                if (isStem.Equals("yes"))
                {
                    word = stem.step1(entry.Key);
                }

                if (word.Equals(keyword))
                {
                    foreach (DocumentNode list in entry.Value.getDocuments())
                    {
                        count++;
                        info += "\n[DocID: " + list.documentID + " Line #: " + list.lineNum + " Word Position: " + list.wordNum + "]";
                        info += "\nDoc [DocID: " + list.documentID + "] line: " + list.line + "\n\n";

                    }
                }
            }


            return info + "\nFreq:" + count;
        }


        public string retrieveDocInfo(string docNum)
        {

            string info = "";
            info += "DOC: [" + docNum + "] ";
            return info;
        }

        public void calcWeights(string docNum)
        {

            ArrayList weights = new ArrayList();
            SortedDictionary<string, double> temp2 = new SortedDictionary<string, double>(); //for query

            foreach (KeyValuePair<string, Term> entry in termDict)
            {
                ArrayList temp = new ArrayList();

                //ignore all the terms that are not in the query for calculation:- reduces time for unwanted similarities.
                if (query.ContainsKey((entry.Key)))
                {
                    foreach (KeyValuePair<string, DocumentNode> doc in entry.Value.getDocumentHalf())
                    {

                        if (doc.Value.documentID.Equals(docNum))
                        {
                            // System.Console.WriteLine(entry.Key + ": DOC: [" + docNum + "] wtf[" + doc.Value.wtf + "] idf[" + doc.Value.idf + "] weight [" + doc.Value.weight + "]");
                            temp.Add(doc.Value.idf);
                            weights.Add(doc.Value.weight);
                        }

                    }

                    if (query.ContainsKey(entry.Key) == false)
                        temp2.Add(entry.Key, 0);
                    else
                        temp2.Add(entry.Key, query[entry.Key]);
                }
            }

            qDocs.Add(docNum, temp2); //document # along with query term frequency.
            DocsWeights.Add(docNum, weights);

            //get doc scalars >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            double sum = 0;
            foreach (double weight in weights)
            {
                sum += (weight * weight);
            }

            sum = Math.Sqrt(sum);
            doc_norm.Add(docNum, sum);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        }


        public void calcQWeights(string docNum)
        {

            ArrayList qweights = new ArrayList();
            foreach (KeyValuePair<string, double> entry in query)
            {
                double idf = Math.Log(3204 / termDict[entry.Key].getDocumentHalf().Count);
                double wtf = 0;

                if (entry.Value > 0)
                {
                    wtf = (1 + Math.Log(entry.Value));
                }
                qweights.Add(wtf * idf);

            }

            QWeights.Add(docNum, qweights);



            double sum = 0;
            //calculate query scalar
            foreach (double weight in qweights)
            {
                sum += (weight * weight);
            }

            sum = Math.Sqrt(sum);
            q_norm.Add(docNum, sum);
        }


        public void calcNWeights()
        {
            foreach (KeyValuePair<string, ArrayList> entry in DocsWeights)
            {
                ArrayList temp = new ArrayList();
                foreach (double weight in entry.Value)
                {
                    System.Console.WriteLine(weight / doc_norm[entry.Key]);

                    temp.Add((weight / doc_norm[entry.Key]));
                }
                DocsNWeights.Add(entry.Key, temp);
            }
        }


        public void calcQNWeights()
        {
            foreach (KeyValuePair<string, ArrayList> entry in QWeights)
            {
                ArrayList temp = new ArrayList();
                foreach (double weight in entry.Value)
                {
                    System.Console.WriteLine(weight / q_norm[entry.Key]);

                    temp.Add((weight / q_norm[entry.Key]));

                }


                QNWeights.Add(entry.Key, temp);

            }

        }


        public void calcSim()
        {

            foreach (KeyValuePair<string, ArrayList> entry in DocsWeights)
            {
                int count = 0;
                double sim = 0;
                ArrayList qnw = new ArrayList();
                qnw = QNWeights[entry.Key];

                foreach (double nw in entry.Value)
                {
                    object o = qnw[count];

                    sim += ((double)o * nw);
                    count++;
                }
                docSim.Add(entry.Key, sim);
            }
        }

        public string getOutputText() {

            return outputText;
        }

        public void displaySim()
        {
            double average = 0;
            int docCount = 0;
            /*risk
            foreach (KeyValuePair<string, double> entry in docSim)
            {
                average += entry.Value;
            }
            average = average / docSim.Count;
            */

            SortedDictionary<double, string> score = new SortedDictionary<double, string>();

            for (int count = 0; count < docSim.Count; count++)
            {
                double highest = 0;
                string doc = "";

                foreach (KeyValuePair<string, double> entry in docSim)
                {
                    if (entry.Value > highest)
                    {
                        highest = entry.Value;
                        doc = entry.Key;
                    }

                   // score.Add(entry.Value, entry.Key);

                }
                docSim.Remove(doc);
                file3.WriteLine("---------------------------------- SIM: {0}-------------------------------------------------------", highest);
                file3.WriteLine(retrieveDocInfo(doc));
                System.Console.WriteLine(retrieveDocInfo(doc));
                docCount++;
                System.Console.WriteLine(docCount);
            }

            outputText = "" + docCount;
        }


        public void displayDocValues(string docNum)
        {

            foreach (KeyValuePair<string, Term> entry in termDict)
            {
                //ignore all the terms that are not in the query for calculation:- reduces time for unwanted similarities.
                if (query.ContainsKey((entry.Key)))
                {
                    foreach (KeyValuePair<string, DocumentNode> doc in entry.Value.getDocumentHalf())
                    {
                        if (doc.Value.documentID.Equals(docNum))
                        {
                            System.Console.WriteLine(entry.Key + ": wtf[" + doc.Value.wtf + "] idf[" + doc.Value.idf + "] weight [" + doc.Value.weight + "]  n-weight[" + DocsNWeights[docNum] + "]");
                        }

                    }
                }
            }
        }

        public SortedDictionary<string, DocumentNode> retrieveDocs(string keyword)
        {
            string info = "";

            keyword = keyword.ToLower();
            return termDict[keyword].getDocumentHalf();
        }


        public void parseQuery(string line)
        {
            

            var words = line.Split(' ');
            clearAll();
            Stem stem = new Stem();

            //calculate term frequency of query.
            foreach (string word in words)
            {
                System.Console.WriteLine("PASSS 1");
                string w = Regex.Replace(word, @"[^\w]", string.Empty);

                if (isStem.Equals("yes"))
                    w = stem.step1(w);

                if (isStopWord.Equals("yes") && checkStopWord(w)) {
                    if (termDict.ContainsKey(w) == true)
                    {
                        if (query.ContainsKey(w) == false)
                            query.Add(w, 1);
                        else
                            query[w] = query[w] + 1;
                    }
                }

                else if (isStem.Equals("no") && isStopWord.Equals("no"))
                {
                    if (termDict.ContainsKey(w) == true)
                    {
                        if (query.ContainsKey(w) == false)
                            query.Add(w, 1);
                        else
                            query[w] = query[w] + 1;
                    }

                }

                else if (isStopWord.Equals("no") && checkStopWord(w))
                {
                    if (termDict.ContainsKey(w) == true)
                    {
                        if (query.ContainsKey(w) == false)
                            query.Add(w, 1);
                        else
                            query[w] = query[w] + 1;
                    }

                }

                else{
                    if (termDict.ContainsKey(w) == true)
                    {
                        if (query.ContainsKey(w) == false)
                            query.Add(w, 1);
                        else
                            query[w] = query[w] + 1;
                    }
                }
            }

            System.Console.WriteLine("PASSS XXX");
            foreach (KeyValuePair<string, double> entry in query)
            {
                System.Console.WriteLine(">>>>>>>>{0} {1}", entry.Key, entry.Value);

            }


            outputDocumentTable2(line);

        }

        private void outputScalar()
        {
            foreach (KeyValuePair<string, double> entry in doc_norm)
            {
                System.Console.WriteLine("doc:{0} => |{1}| ===========>", entry.Key, entry.Value);
            }
        }


        private void outputQueryTermFreq()
        {
            foreach (KeyValuePair<string, double> entry in allTerms)
            {
                System.Console.WriteLine("word: {0} => {1}", entry.Key, entry.Value);
            }
        }


        public void outputDocumentTable2(string line)
        {
            var words = line.Split(' ');


            System.Console.WriteLine("Retrieving Doc....");
            Stem stem = new Stem();


            int count = 0;
            foreach (string word in words)
            {
                //query.Add(word, 0);
                string w = Regex.Replace(word, @"[^\w]", string.Empty);

                count++;
                if (isStem.Equals("yes"))
                    w = stem.step1(w);

                if (isStem.Equals("yes"))
                {

                    foreach (KeyValuePair<string, Term> entry in termDict)
                    {
                        if (isStopWord.Equals("no") && checkStopWord(entry.Key)) { }

                        else if (isStopWord.Equals("yes") && checkStopWord(entry.Key))
                        {
                            if (entry.Key.Equals(word) || stem.step1(entry.Key).Equals(w))
                            {
                                if (termDict.ContainsKey(entry.Key) == true)
                                {
                                    if (query.ContainsKey(entry.Key) == false)
                                        query.Add(entry.Key, 1);
                                    else
                                        query[entry.Key] = query[entry.Key] + 1;
                                }
                                System.Console.WriteLine("**{0} => {1}", entry.Key, w);
                                System.Console.WriteLine("{0} => {1}", stem.step1(entry.Key), w);
                                relevantDocs.Add(count.ToString(), retrieveDocs(entry.Key));
                                count++;
                            }
                        }

                        else
                        {
                            if (entry.Key.Equals(word) || stem.step1(entry.Key).Equals(w))
                            {
                                if (termDict.ContainsKey(entry.Key) == true)
                                {
                                    if (query.ContainsKey(entry.Key) == false)
                                        query.Add(entry.Key, 1);
                                    else
                                        query[entry.Key] = query[entry.Key] + 1;

                                    System.Console.WriteLine("**{0} => {1}", entry.Key, w);
                                    System.Console.WriteLine("{0} => {1}", stem.step1(entry.Key), w);
                                    relevantDocs.Add(count.ToString(), retrieveDocs(entry.Key));
                                    count++;
                                }
                            }

                        }
                    }
                }
               

                if (isStem.Equals("no") && termDict.ContainsKey(w))

                {
                    if (isStopWord.Equals("yes") && checkStopWord(w) && termDict.ContainsKey(w))
                        relevantDocs.Add(count.ToString(), retrieveDocs(w));

                    else if (isStopWord.Equals("no") && checkStopWord(w))
                    { }
                    else {
                        relevantDocs.Add(count.ToString(), retrieveDocs(w));
                    }
                }

            }

            //AND all the relevant docs together to one>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            foreach (KeyValuePair<string, SortedDictionary<string, DocumentNode>> entry in relevantDocs)
            {

                foreach (KeyValuePair<string, DocumentNode> doc in entry.Value)
                {

                    if (relevantDocsList.ContainsKey(doc.Value.documentID) == false)
                        relevantDocsList.Add(doc.Value.documentID, doc.Value);
                }
            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //calculate weights for each document in the relevant list.
            foreach (KeyValuePair<string, DocumentNode> entry in relevantDocsList)
            {
                calcWeights(entry.Key);
                calcQWeights(entry.Key);

            }

            //calculate n-weights for each document in the relevant list.
            calcNWeights();
            calcQNWeights();


            foreach (KeyValuePair<string, DocumentNode> entry in relevantDocsList)
            {
                System.Console.WriteLine("----------------------{0}----------------------------", entry.Key);
                System.Console.WriteLine("PASSS 1");
                displayDocValues(entry.Key);
                System.Console.WriteLine("");
            }

            calcSim();
           displaySim();
           table();
        }


        public void table()
        {

            foreach (KeyValuePair<string, SortedDictionary<string, ArrayList>> entry in relevantDocsTable)
            {
                file2.WriteLine("------------------------DOC: {0}----------------------------", entry.Key);
                foreach (KeyValuePair<string, ArrayList> tableTerm in entry.Value)
                {

                    file2.Write("{0} {1} ",
                        tableTerm.Key, tableTerm.Value[0]);

                    file2.Write("{0} {1} ",
                           tableTerm.Value[1], tableTerm.Value[2]);

                    file2.Write("{0} {1}",
                           tableTerm.Value[3], tableTerm.Value[4]);
                    file2.WriteLine("");
                }

            }
            file2.Close();
        }

        /*
      * ******************************************** checkStopWord(string word) ******************************************** 
      * Checks to see if a word is a stop-word.
      * ********************************************************************************************************
      */
        public bool checkStopWord(string word)
        {
            if (stopWordList.Contains(word))
                return true;
            else
                return false;
        }

        public void loadStopWord()
        {
            StreamReader stopWordRead = new StreamReader(path + "\\common_words");
            // string path = Directory.GetCurrentDirectory();
            // System.Console.WriteLine(path);
            // StreamReader stopWordRead = new StreamReader(""+path+"\\common_words");
            try
            {
                do
                {
                    string word = stopWordRead.ReadLine();
                    stopWordList.Add(word);
                } while (stopWordRead.Peek() != -1);
            }
            catch
            {
                System.Console.WriteLine("Stop Word File not found! Change directory in code...");
            }
            finally
            {
                stopWordRead.Close();
            }

        }

        public void clearAll()
        {
            query.Clear();
            qDocs.Clear();
            relevantDocsList.Clear();
            relevantDocsTable.Clear();
            relevantDocs.Clear();
            doc_norm.Clear();
            doc_nw.Clear();
            q_nw.Clear();
            DocsWeights.Clear();
            DocsNWeights.Clear();
            qWeights.Clear();
            qNWeights.Clear();
            qidf.Clear();
            qDocs.Clear();
            qNWeights.Clear();
            QWeights.Clear();
            QNWeights.Clear();
            q_norm.Clear();
            docSim.Clear();
        }
    }

}
