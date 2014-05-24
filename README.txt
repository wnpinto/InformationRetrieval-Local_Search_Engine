====About the Project=====

This program is a local search engine that searches for specified documents in PDF, HTML, TXT or DOC formats.

The search engine implements the vector space model for information classification and retrieval.



*This program is written in C#



===========================
How to run the program
===========================
The program is coded in C# and tested on Visual Studio Express 2012. 
Running the invert:
Goto .\CPS842_InformationRetrievalAssignment2\bin\debug\ directory
Then run the CPS842_InformationRetrievalAssignment2.exe file
-	The program will first ask if you want to enable stop words. Type yes/no .
-	The program will then ask if you want to enable stemming. Type yes/no .
-	The program will then output the doc IDs that it traverses so you can visually see that the program is running. Once it is done reading the last doc, the program will create the postings and dictionary files along with the object data file.
Running the test:
-	Goto .\ConsoleApplication1\bin\debug\ directory
-	Then run the ConsoleApplication1.exe file
-	The program will first de-serialize the data
-	The program will then ask the user for a word to lookup.
-	The program will then calculate the similarity scores and output to a file named "docSim.txt" in the "\ConsoleApplication1\bin\Debug" folder
-	Based on the query, the program may return a list of documents in decreasing order of similarity scores.
