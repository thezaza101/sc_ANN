using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace helpers
{
    public class MatrixData<T>
    {
        //This represents the headers of the maxtrix information
        public string[] Headers
        {
            get
            {
                return _headers;
            }
        }
        public T[,] Data
        {
            get
            {
                return _data;
            }
        }

        public int NumberOfRows{get;private set;}
        public int NumberOfColumns{get;private set;}


        private string[] _headers;
        private T[,] _data;

        public MatrixData()
        {

        }

        public MatrixData(string filelocation, bool hasHeaders = true, char delimiter = ',')
        {
            ReadFromCSV(filelocation, hasHeaders,delimiter);
        }

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

        public void SplitData(out MatrixData<T> SplitTo, int row, int col)
        {
            throw new NotImplementedException();
        }

        public void CopyData(out MatrixData<T> CopyTo, int row, int col)
        {
            throw new NotImplementedException();
        }

        public void AddRow(T[] newRow)
        {
            if (newRow.Length != NumberOfColumns)
            {
                throw new Exception("Number of columns in the new row ("+newRow.Length+") must equal to the number of columns{"+NumberOfColumns+")");
            }
            throw new NotImplementedException();   
        }

        public string Head(int numberOfRows = 5, int colWidth = 10)
        {
            string output ="";
            foreach (string h in _headers)
            {
                output += ColValue(h) + '|';
            }
            output += Environment.NewLine;
            output += new String('-', NumberOfColumns*colWidth).ToString();
            
            for (int r = 0; r<numberOfRows;r++)
            {
                output += Environment.NewLine;
                for (int c = 0; c < NumberOfColumns; c++)
                {
                    output += ColValue(_data[r,c].ToString()) + '|';
                }
            }

            string ColValue(string value)
            {
                int valLength = value.Length;
                string valueToWrite = "";
                if (valLength > colWidth-4)
                {
                    valueToWrite = value.Substring(0,colWidth-4);
                }
                else
                {
                    if(valLength%2==0)
                    {
                        int numspaces = ((colWidth-4) - valLength)/2;
                        valueToWrite = (new String(' ', numspaces)) + value + (new String(' ', numspaces));
                    }
                    else
                    {
                        int numspaces = ((colWidth-4) - valLength-1)/2;
                        valueToWrite = (new String(' ', numspaces)) + value + (new String(' ', numspaces))+" ";
                    }                    
                }
                return " "+valueToWrite+" ";
            }
            return output;
        }

        //attempt to normalize a numaric column
        public void Normalize(int col, NormalizationMethod method = NormalizationMethod.FeatureScalingMinMax)
        {
            Type t = _data[0,col].GetType();
            List<Type> numaricTypes = new List<Type>();
            numaricTypes.Add(typeof(double));
            numaricTypes.Add(typeof(int));
            numaricTypes.Add(typeof(decimal));
            if(numaricTypes.Contains(t))
            {

                //MinMax
                //https://docs.microsoft.com/en-us/azure/machine-learning/studio-module-reference/normalize-data
                //https://en.wikipedia.org/wiki/Normalization_(statistics)
                //https://en.wikipedia.org/wiki/Feature_scaling
                double min = double.Parse(_data[0,col].ToString());
                double max = double.Parse(_data[0,col].ToString());
                
                //find the minimum and maximum value in the column
                for (int row = 1; row < NumberOfRows; row++)
                {
                    double currentVal = double.Parse(_data[row,col].ToString());
                    min = (currentVal < min)? currentVal : min;
                    max = (currentVal > max)? currentVal : max;
                }

                double mult = 1 / (max - min);
                
                for (int row = 0; row < NumberOfRows; row++)
                {
                    double currentVal = double.Parse(_data[row,col].ToString());
                    currentVal = (currentVal - min) * mult;
                    _data[row,col] = Convert(currentVal.ToString());
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

        //https://stackoverflow.com/questions/2961656/generic-tryparse
        //This method will try parse the string data to the Type specified when the class was created
        private T Convert(string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if(converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }
    }
}
