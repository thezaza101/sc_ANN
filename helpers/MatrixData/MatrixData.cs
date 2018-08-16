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

        public MatrixData(System.Data.DataTable dt)
        {

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

        public void ChangeHeader(int col, string value)
        {
            _headers[col] = value;
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

        //https://stackoverflow.com/questions/30164019/shuffling-2d-array-of-cards
        public void Suffle()
        {
            Random random = new Random();
            T[][] data = ToJagged(_data);
            data = data.OrderBy(t => random.Next()).ToArray();
            _data = ToRectangular(data);
        }

        public void Sort(int col, bool acen = true)
        {
            if(acen)
            {
                _data = ToRectangular(ToJagged(_data).OrderBy(t => t[col]).ToArray());
            }
            else
            {
                 _data = ToRectangular(ToJagged(_data).OrderByDescending(t => t[col]).ToArray());
            }
        }
        

        //https://stackoverflow.com/questions/33417721/convert-a-object-array-into-a-dataset-datatable-in-c-sharp
        public System.Data.DataTable ToDataTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            for (int col = 0; col < _data.GetLength(1); col++)
            {
                dt.Columns.Add(_headers[col]);
            }

            for (var row = 0; row < _data.GetLength(0); ++row)
            {
                System.Data.DataRow r = dt.NewRow();
                for (var col = 0; col < _data.GetLength(1); ++col)
                {
                    r[col] = _data[row, col];
                }
                dt.Rows.Add(r);
            }
            return dt;
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

        //https://stackoverflow.com/questions/232395/how-do-i-sort-a-two-dimensional-array-in-c
        //Convert rectangular array to jagged array
        private T[][] ToJagged(T[,] array) 
        {
            int height = array.GetLength(0); 
            int width = array.GetLength(1);
            T[][] jagged = new T[height][];

            for (int i = 0; i < height; i++)
            {
                T[] row = new T[width];
                for (int j = 0; j < width; j++)
                {
                    row[j] = array[i, j];
                }
                jagged[i] = row;
            }
            return jagged;
        }
        //Convert jagged array to rectangular array
        private T[,] ToRectangular(T[][] array)
        {
            int height = array.Length;
            int width = array[0].Length;
            T[,] rect = new T[height, width];
            for (int i = 0; i < height; i++)
            {
                T[] row = array[i];
                for (int j = 0; j < width; j++)
                {
                    rect[i, j] = row[j];
                }
            }
            return rect;
        }
    }
}
