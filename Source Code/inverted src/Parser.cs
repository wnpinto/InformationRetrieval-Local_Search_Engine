/*
 * 
 * Thusitharan Paramsothy, 500275858
 * Warren Pinto, 500396119
 * 
 * PARSER CLASS -> Creates inverted index
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text.pdf.parser;
using System.util.collections;
using iTextSharp.text;
using Microsoft.Office.Interop.Word;

namespace CPS842_InformationRetrivalAssignment2
{
    [Serializable()]
    class Parser
    {
        Stemmer s = new Stemmer();
        string file;    //collection file
        string author;
        string title; 
        int docID;
        int lineNum;
        int lineNumber;
        int wordNum;
        String docIDx;
        String docIDpattern = @"\.I\s[0-9]*"; // test for .I 100 (.I [any number]) - document ID
        String docTitlepattern = @"\.T"; // test for .T - document Title
        String docAbstractpattern = @"\.W"; // test for .T - document Title
        String docAuthorpattern = @"\.A"; // test for .T - document Title
        ArrayList stopWordList = new ArrayList();
        bool readAbstract = false;
        bool stemming = false;
        bool stopW = false;
        private string text {get; set;}
        DocumentNode node = new DocumentNode();
        //Instantiate a Hashtable: The table will hold term-postings (key-value) pairs.
        Hashtable dictionary = new Hashtable();
        string path = Directory.GetCurrentDirectory();
        SortedDictionary<string, ArrayList> sortedDict = new SortedDictionary<string, ArrayList>();
        SortedDictionary<string, Term> termDict = new SortedDictionary<string, Term>();
        SortedDictionary<string, DocumentNode> docList = new SortedDictionary<string, DocumentNode>();

        public Parser(string fileDir, int docID, bool stopWord, bool stem) {
            file = fileDir;
            this.docID = docID;
            stopW = stopWord;
                loadStopWord();

            stemming = stem;
            //retrieveData();


           

        }


        /*
         * ******************************************** retrieveData() ******************************************** 
         * Reads the collection file. Then creates an inverted index.
         * ********************************************************************************************************
         */

        public void retrieveData() {
            StreamReader reader = new StreamReader(file);
            Regex rgxDocID = new Regex(docIDpattern, RegexOptions.IgnoreCase);
            Regex rgxDocTitle = new Regex(docTitlepattern, RegexOptions.IgnoreCase);
            Regex rgxDocAbstract = new Regex(docAbstractpattern, RegexOptions.IgnoreCase);
            Regex rgxDocAuthor = new Regex(docAuthorpattern, RegexOptions.IgnoreCase);
            Regex rgxNum = new Regex(docAbstractpattern, RegexOptions.IgnoreCase);
           
            try
            {
                do
                {
                    String line = (reader.ReadLine());
                    lineNum++;
                    lineNumber++;
                   
                    //match if line has ".I 0000". To retrieve document ID from line
                    if (rgxDocID.IsMatch(line, 0)){
                        readAbstract = false;
                        var x = line.Split(' ');
                        docIDx = x.ElementAt<string>(1); //sets docIDx = document # in collection
                        System.Console.WriteLine("DOC ID: {0}", docIDx);
                        lineNum = 0;
                        docList.Add(docIDx, new DocumentNode());
                    }

                    //match if line has ".A". To retrieve author information from line.
                    else if (rgxDocAuthor.IsMatch(line, 0)){
                        readAbstract = false;
                        author = (reader.ReadLine());
                        lineNumber++;                       
                        lineNum = 0;
                    }

                    //match: To retrieve title information from line.
                    else if(rgxDocTitle.IsMatch(line, 0)){
                        line = (reader.ReadLine());
                        title = line;
                        lineNumber++;
                        var words = line.Split(); //split sentance into words. 
                        wordNum = 0;


                        //loop: For each word in the sentence, remove unwanted charecters, do the stemming (if enabled), then insert word into index.
                        foreach (string word in words){                      
                                 string stripedWord = Regex.Replace(word, @"[^\w]", string.Empty);
                                
                                wordNum++;
                                if (stemming){
                                    Porter porter = new Porter();
                                    stripedWord = porter.checkEndPunc(stripedWord.ToCharArray());
                                }

                                    insertWord(stripedWord, line); //insert word into index with its line number.
                        }
              

                        System.Console.WriteLine("Title: {0}", line);
                    }

                    //condition to set if the parser is currently reading the abstract. Used to perfom looping for reading abstract only.
                    else if (rgxDocAbstract.IsMatch(line, 0))
                        readAbstract = true;
        

                    else {
                        if (readAbstract){

                            wordNum = 0;
                            var words = line.Split();

                            foreach (string word in words){

                               string stripedWord = Regex.Replace(word, @"[^\w]", string.Empty);
                               
                                if (stemming)
                               {
                                  // Porter porter = new Porter();
                                   //stripedWord = porter.checkEndPunc(stripedWord.ToCharArray());
                                   Stem stem = new Stem();
                                   stripedWord = stem.step1(stripedWord);
                               }
                                wordNum++;

                                    insertWord(stripedWord, line);
                            }
                        }
                    }
                    docList[docIDx].author = author;
                    docList[docIDx].title = title;
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


        /*
        * ******************************************** checkStopWord(string word) ******************************************** 
        * Checks to see if a word is a stop-word.
        * ********************************************************************************************************
        */
        public bool checkStopWord(string word) {
            if (stopWordList.Contains(word))
                return true;
            else 
                return false;
        }


        /*
        * ******************************************** insertWord(string word, string line) ******************************************** 
        * Inserts a word into the inverted index.
        * *******************************************************************************************************************************
        */
        public void insertWord(string word, string line){
            word = word.ToLower();

            if (checkStopWord(word) && stopW == false){}


            else
            {
                if (!(word.Equals("")))
                {

                    //if (word.Length > 3)
                    // word = porter.checkEnd(word.ToCharArray());

                    if (sortedDict.ContainsKey(word))
                    {
                        ArrayList temp = (ArrayList)sortedDict[word];
                        termDict[word].frequency = (termDict[word].frequency + 1); // add 1 to the term frequency
                        termDict[word].addLine(line); // add the line to the term 
                        termDict[word].docLineNum = (lineNumber); // add the line #

                        if (!(termDict[word].getDocumentIDList().Contains(docIDx))){
                            termDict[word].addDocument(docIDx);
                        }


                        if (termDict[word].getDocumentHalf().ContainsKey(docIDx))
                            termDict[word].getDocumentHalf()[docIDx].frequency++;
                        else
                            termDict[word].addDocumentHalf(new DocumentNode(docIDx,1, word));

                        temp.Add(new DocumentNode(docIDx, 2, lineNum, wordNum, line, author, word));
                        termDict[word].addDocument(new DocumentNode(docIDx, 2, lineNum, wordNum, line, author, word));
                        sortedDict[word] = temp;


                        wordNum++;
                    }

                    else
                    {
                        ArrayList list = new ArrayList();
                        list.Add(new DocumentNode(docIDx, 1, lineNum, wordNum, line, author, word));
                        sortedDict.Add(word, list); //add [word] -> [list] to the main hash

                        termDict.Add(word, new Term(word, 1));
                        //termDict[word].frequency = (termDict[word].frequency + 1);
                      //  termDict[word].addLine(line);
                      //  termDict[word].docLineNum = (lineNumber);
                        termDict[word].addDocumentHalf(new DocumentNode(docIDx, 1, word));
                    }
                }
            }
        }


        public void loadStopWord(){
            StreamReader stopWordRead = new StreamReader(path + "\\common_words");
           // string path = Directory.GetCurrentDirectory();
           // System.Console.WriteLine(path);
           // StreamReader stopWordRead = new StreamReader(""+path+"\\common_words");
            try
            {
                do{
                 string word = stopWordRead.ReadLine();
                 stopWordList.Add(word);
                } while (stopWordRead.Peek() != -1);
             }
            catch{
                System.Console.WriteLine("Stop Word File not found! Change directory in code...");
            }
            finally{
                stopWordRead.Close();
             }
        
        }


        public void printList() {
            foreach ( KeyValuePair<string, Term> entry in termDict)
            {
               
                object tempO = entry.Value;
               // ArrayList temp = (ArrayList)tempO;
                object key = entry.Key;
             
                float frequency = termDict[entry.Key].frequency;
                
                Console.Write("Term: {0}[{1}]", entry.Key, frequency);
                Console.Write("*{0}*", termDict[entry.Key].getDocumentsString());
                Console.WriteLine("");
	        }
            
            Console.WriteLine("");
            
        }

        
        public string tokenize(string word) {
            return word;
        }

        public SortedDictionary<string, ArrayList> getPostingsDictionary() {
            return sortedDict;
        }

        public string retrieveDocumentLine(int lineN)
        {
            StreamReader fileRead = new StreamReader(file);
            int line = 0;
            string linew = "";
            try
            {
                do
                {
                    linew = fileRead.ReadLine();
                  
                    if ((lineN -1 )== line) {
                        break;
                    }
                    else
                        line++;

                   
                } while (fileRead.Peek() != -1);

            }
            catch
            {

                System.Console.WriteLine("Stop Word File not found!");
            }
            finally
            {
                fileRead.Close();
            }

            return linew;
        }

        public string retrieveTermInfo(string keyword)
        {
            string info = "";

            keyword = keyword.ToLower();
          //  if (keyword.Length > 3)
            //    keyword = porter.checkEnd(keyword.ToCharArray());

            info += "\nTERM:  " + keyword;
            info += "\nTerm Frequency: " + termDict[keyword].frequency;

            foreach (DocumentNode list in (termDict[keyword].getDocuments()))
            {
               
               info += "\n[DocID: " + list.documentID + " Line #: " + list.lineNum + " Word Position: " + list.wordNum + "]";
               info += "\nDoc [DocID: " + list.documentID + "] line: " + list.line + "\n\n";
                   
            }

            return info + "\nFreq:" + termDict[keyword].frequency;
        }

        public void outputPostings()
        {
            string line = "";
             System.Console.WriteLine("\n\nOutputting to files....\n\n");

             System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\postings.txt");
            
            foreach( KeyValuePair<string, Term> entry in termDict)
            {
                line = ""+entry.Key + " ----- " + termDict[entry.Key].getDocumentsString();
                file.WriteLine(line);
            }

            file.Close();
        }

        public void outputDictionary()
        {
            string line = "";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\Dictionary.txt");

            foreach (KeyValuePair<string, Term> entry in termDict)
            {
                line = "" + entry.Key + " [" + entry.Value.frequency + "]";
                file.WriteLine(line);
            }
            file.Close();
        }

        public void calculate()
        {
            foreach (KeyValuePair<string, Term> entry in termDict)
            {
                foreach (KeyValuePair<string, DocumentNode> doc in entry.Value.getDocumentHalf()) {
                    double wtf = (1 + Math.Log(doc.Value.frequency));
                    double idf = (Math.Log(3204 / (entry.Value.getDocumentHalf().Count)));

                    doc.Value.wtf = wtf;
                    doc.Value.idf = idf;
                    doc.Value.weight = (wtf * idf);
                }
            }
        }

        public void outputAdvancedPostings()
        {
            string line = "";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\AdvancedPostings.txt");

            foreach (KeyValuePair<string, Term> entry in termDict)
            {
                line = "" + entry.Key + " " + termDict[entry.Key].getDocumentsString();
                file.WriteLine(line);
            }
        }

        public void serializeData() {
            calculate(); //caluelate tf, idf ..... for each term.
            displayDocValues();

            string p = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            p = Directory.GetParent(p).ToString();
            p = Directory.GetParent(p).ToString();

            Stream fileStream = File.Create(p + "\\ConsoleApplication1\\bin\\Debug\\dataObj");
            Stream fileStream2 = File.Create(p + "\\ConsoleApplication1\\bin\\Debug\\dataObj2");
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(fileStream, termDict);
            serializer.Serialize(fileStream2, docList);
            fileStream.Close();
        }

        public void displayDocValues()
        {
            string docNum = "2104";

            foreach (KeyValuePair<string, Term> entry in termDict) {
                foreach (KeyValuePair<string, DocumentNode> doc in entry.Value.getDocumentHalf()) {
                    if (doc.Value.documentID.Equals(docNum)) {
                        System.Console.WriteLine(entry.Key + ": wtf[" + doc.Value.wtf + "] idf[" + doc.Value.idf+ "]");
                    }

                }
            
            }

        }

        public string extractText(string path)
        {
            StreamReader reader = new StreamReader(path);
            string plainText = "";
            string line = "";

            try
            {
                do
                {
                    line = reader.ReadLine();
                    line = Regex.Replace(line, "-", " ");
                    plainText += Regex.Replace(line, "<[^>]+?>", "");

                } while (reader.Peek() != -1);
            }
            catch
            {

                System.Console.WriteLine("Text File not found!");
            }
            finally
            {
                reader.Close();
            }

            return plainText;
        }


        public string extractTextHTML(string path)
        {
            
            StreamReader reader = new StreamReader(path);
            string plainText = "";
            string line = "";
           // Regex.Replace(htmlText, "<[^>]+?>", "");

            try
            {
                do
                {
                    line = reader.ReadLine();
                    line = Regex.Replace(line, @"\s+", " ");
                    plainText+=Regex.Replace(line, "<[^>]+?>", "");

                } while (reader.Peek() != -1);
            }
            catch
            {

                System.Console.WriteLine("HTML File not found!");
            }
            finally
            {
                reader.Close();
            }

            return plainText;
        }


        public string extractTextPDF(string path)
        {
            string text = "";
            try
            {
                PdfReader reader = new PdfReader(path);
                for(int index = 1; index < reader.NumberOfPages; index++)
                    text += PdfTextExtractor.GetTextFromPage(reader, index, new LocationTextExtractionStrategy());

               // System.Console.WriteLine("FROM PDF: {0}", text);
            }
            catch
            {

                System.Console.WriteLine("PDF File not found!");
            }
            finally
            {
               // reader.Close();
            }


            return text;
        }

        public string extractTextDOC(string path)
        {
            string text = "";
            Microsoft.Office.Interop.Word.Application wordApp = new Application();
            //Word.ApplicationClass is to access the word application

            
            object file = path;
            object nullobj = System.Reflection.Missing.Value;

            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(ref file, ref nullobj, ref nullobj,
                ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj,
                ref nullobj, ref nullobj, ref nullobj, ref nullobj, ref nullobj);

            doc.ActiveWindow.Selection.WholeStory();

            doc.ActiveWindow.Selection.Copy();
            text = doc.Content.Text;
            doc.Close();
            wordApp.Quit();

            return text;
        }

        public void retrieveFiles() {

            string path = @"C:\Users\fs1071\Desktop\Collection\";
            string[] pdf_files = Directory.GetFiles(path, @"*.pdf", SearchOption.AllDirectories);
            string[] html_files = Directory.GetFiles(path, @"*.html", SearchOption.AllDirectories);
            string[] DOC_files = Directory.GetFiles(path, @"*.docx", SearchOption.AllDirectories);
            string[] text_files = Directory.GetFiles(path, @"*.txt", SearchOption.AllDirectories);
            
            string[] files = new string[pdf_files.Length + html_files.Length + DOC_files.Length + text_files.Length];
            pdf_files.CopyTo(files, 0);
            html_files.CopyTo(files, 0);
            DOC_files.CopyTo(files, 0);
            text_files.CopyTo(files, 0);
 
            string text = "";
            
            foreach (string file in files) {
                System.Console.WriteLine(file);

                if (file != null)
                {
                    Regex pdfRegx = new Regex(@"[\w]*\.pdf", RegexOptions.IgnoreCase);
                    Regex htmlRegx = new Regex(@"[\w]*\.html", RegexOptions.IgnoreCase);
                    Regex DocRegx = new Regex(@"[\w]*\.docx", RegexOptions.IgnoreCase);
                    Regex TextRegx = new Regex(@"[\w]*\.txt", RegexOptions.IgnoreCase);

                    if (pdfRegx.IsMatch(file, 0))
                        text = extractTextPDF(file);
                    else if (htmlRegx.IsMatch(file, 0))
                        text = extractTextHTML(file);
                    else if (DocRegx.IsMatch(file, 0))
                    {
                        System.Console.WriteLine("PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP");
                        text = extractTextDOC(file);
                    }
                    else if (TextRegx.IsMatch(file, 0))
                        text = extractText(file);

                    var words = text.Split(' ');
                    string[] fileX = file.Split('\\');
                    docIDx = file;
                    System.Console.WriteLine("\n\n*****FILE: {0}******", file);


                    foreach (string word in words)
                    {

                        string stripedWord = Regex.Replace(word, @"[^\w]", string.Empty);
                        // Regex digit = new Regex(@"[\d]*", RegexOptions.IgnoreCase);

                        if (Regex.IsMatch(stripedWord, @"^\d.*")) { }
                         else
                            insertWord(stripedWord, "1");
                    }
                }
            }

            foreach (KeyValuePair<string, Term> entry in termDict) {
                System.Console.WriteLine("TERM: {0}  ===== FREQ: {1}", entry.Value.getTerm(), entry.Value.frequency);
               
            }
        
        }

    }




}
