/*
 * 
 * Thusitharan Paramsothy, 500275858
 * Warren Pinto, 500396119
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CPS842_InformationRetrivalAssignment2
{
    class Porter
    {
        char[] wordArray;

        public Porter() { 
        
        
        }

        public Porter(string word) {
            wordArray = word.ToCharArray();    
        }


        public void stripEnd(char[] word) {
            checkEnd(word);
        
        }

        public string checkEnd(char[] word)
        {
            /* string ss = new String(word);
            string pattern = @"[^\w]ing";
             string[] substrings = Regex.Split(ss, pattern);
             return substrings[0];
           */
            for (int i = 0; i < word.GetLength(0); i++)
            {
                string ss = new String(word);
              //  System.Console.WriteLine("TEST: {0}", ss);

                //check ing
                if ((word[word.Length - 4].Equals('i') && word[word.Length - 2].Equals('n') && word[word.Length - 1].Equals('g')))
                {
                    if (word[i - 2].Equals('i') && (word[i - 1].Equals('z') || word[i - 1].Equals('s')))// check for ending iz Serializing -> Serialize
                    {
                        word[i] = 'e';                                        
                        return ss.Remove(word.Length - 2, 2);   
                    }

                    else  // jumping -> jump
                        return ss.Remove(word.Length - 3, 3);
                }

                if (word[word.Length - 2].Equals('e') && word[word.Length - 1].Equals('s'))
                {

                    if (word.Length > 3 && word[word.Length - 3].Equals('s'))// check for ending iz Serializing -> Serialize
                    {

                        return ss.Remove(word.Length - 2, 2);
                    }
                }

            }
            string s = new String(word);
            return s;
          
        }

      
        public string checkEndPunc(char[] word)
        {
            string ss = new String(word);

            if(word.Length > 1)
                     return Regex.Replace(ss, @"[^\w]", "");

           

            return ss;
        }
    }
}
