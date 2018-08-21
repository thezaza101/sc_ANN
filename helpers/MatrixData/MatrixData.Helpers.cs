using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace helpers
{
    public partial class MatrixData
    {
        public string Head(int numberOfRows = 5, int colWidth = 10)
        {
            string output ="";
            foreach (string h in _headers)
            {
                output += ColValue(h) + '|';
            }
            output += Environment.NewLine;

            foreach (Type t in _columnDataTypes)
            {
                output += ColValue(t.Name,'-') + '|';
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

            string ColValue(string value, char fill = ' ')
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
                        valueToWrite = (new String(fill, numspaces)) + value + (new String(fill, numspaces));
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
        private object ConvertToNumeric(string input)
        {
            var converter = TypeDescriptor.GetConverter(DefaultNumericType);
            if (converter != null)
            {
                return converter.ConvertFromString(input);
            }
            return null;
        }

        //https://stackoverflow.com/questions/232395/how-do-i-sort-a-two-dimensional-array-in-c
        //Convert rectangular array to jagged array
        private object[][] ToJagged(object[,] array)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            object[][] jagged = new object[height][];

            for (int i = 0; i < height; i++)
            {
                object[] row = new object[width];
                for (int j = 0; j < width; j++)
                {
                    row[j] = array[i, j];
                }
                jagged[i] = row;
            }
            return jagged;
        }

        //Convert jagged array to rectangular array
        private object[,] ToRectangular(object[][] array)
        {
            int height = array.Length;
            int width = array[0].Length;
            object[,] rect = new object[height, width];
            for (int i = 0; i < height; i++)
            {
                object[] row = array[i];
                for (int j = 0; j < width; j++)
                {
                    rect[i, j] = row[j];
                }
            }
            return rect;
        }
    }
}