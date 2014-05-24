using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Stem
    {
        public string step1(string word)
        {
            //step 1

            string caseIng = @"[\w]+ing";
            string caseIngs = @"[\w]+ings";
            string caseEd = @"[\w]+ed";
            string caseEds = @"[\w]+eds";
            string caseEs = @"[\w]+es";
            string caseE = @"[\w]+e";

            string stemWord;
            if ((Regex.Match(word, caseIng).Success))
            {
                stemWord = word.Substring(0, word.Length - 3);
                return stemWord;
            }
            else if ((Regex.Match(word, caseIngs).Success))
            {
                stemWord = word.Substring(0, word.Length - 3);
                return stemWord;
            }

            else if ((Regex.Match(word, caseEd).Success))
            {
                stemWord = word.Substring(0, word.Length - 2);
                return stemWord;
            }

            else if ((Regex.Match(word, caseEds).Success))
            {
                stemWord = word.Substring(0, word.Length - 2);
                return stemWord;
            }

            else if ((Regex.Match(word, caseEs).Success))
            {
                stemWord = word.Substring(0, word.Length - 2);
                return stemWord;
            }
            else if ((Regex.Match(word, caseE).Success))
            {
                stemWord = word.Substring(0, word.Length - 1);
                return stemWord;
            }

            else
                return word;
        }
    }
}
