using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace helpers
{
    public partial class MatrixData<T>
    {
        //This method reads a CSV file and sets the data.
        public void ReadFromCSV(string filelocation, bool hasHeaders, char delimiter)
        {
            string[] lines = ReadAllLines(filelocation);

            int rowStartIndex = (hasHeaders)? 1 : 0;
            SetHeaders(lines[0], hasHeaders,delimiter);

            NumberOfRows = lines.Length-rowStartIndex;
            NumberOfColumns = _headers.Length;

            _data = new T[NumberOfRows,NumberOfColumns];
            for (int row = rowStartIndex; row<lines.Length; row++)
            {
                string[] data = SplitCsvLine(lines[row],delimiter);
                for(int col = 0; col < NumberOfColumns; col++)
                {
                    _data[row-rowStartIndex,col] = Convert(data[col]);
                }
            }
             
        }

        //This set the header values if the file has headers
        private void SetHeaders(string headersLine, bool hasHeaders, char delimiter)
        {
            if (hasHeaders)
            {
                _headers = SplitCsvLine(headersLine,delimiter);
            }
            else
            {
                _headers = Enumerable.Repeat(string.Empty, SplitCsvLine(headersLine,delimiter).Length).ToArray();
            }         
        }

        //https://stackoverflow.com/questions/12744725/how-do-i-perform-file-readalllines-on-a-file-that-is-also-open-in-excel
        //This method will read the CSV file and split it into lines
        private string[] ReadAllLines(string path)
        {
            using (var csv = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(csv))
                {
                    List<string> file = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        file.Add(sr.ReadLine());
                    }

                    return file.ToArray();
                }
            }
        }

        //https://stackoverflow.com/questions/17207269/how-to-properly-split-a-csv-using-c-sharp-split-function
        //This method will safely split a line of a CSV file
        private string[] SplitCsvLine(string s,char delimiter) 
        {            
            //string pattern = @"""\s*,\s*""";
            //string pattern = @"""\s*"+delimiter+@"\s*""";

            // input.Substring(1, input.Length - 2) removes the first and last " from the string
            //string[] tokens = System.Text.RegularExpressions.Regex.Split(s.Substring(1, s.Length - 2), pattern);
            //return tokens;

            return s.Split(delimiter);            
        }
    }
}
