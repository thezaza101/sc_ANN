using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace helpers
{
    public partial class MatrixData
    {
        //This represents the headers of the maxtrix information
        public string[] Headers
        {
            get
            {
                return _headers;
            }
        }
        public Type[] ColumnDataTypes
        {
            get
            {
                return _columnDataTypes;
            }
        }
        public object[,] Data
        {
            get
            {
                return _data;
            }
        }

        public int NumberOfRows{get;private set;}
        public int NumberOfColumns{get;private set;}
        public Type DefaultNumericType {get; private set;} = typeof(double);

        private string[] _headers;
        private Type[] _columnDataTypes;
        private object[,] _data;

        public MatrixData()
        {

        }
        public MatrixData(string filelocation, bool hasHeaders = true, char delimiter = ',', Type defaultNumericType = null)
        {
            if(!(defaultNumericType==null))
            {
                DefaultNumericType = defaultNumericType;
            }
            ReadFromCSV(filelocation, hasHeaders,delimiter);
        }

        public MatrixData(System.Data.DataTable dt)
        {

        }

        public void SplitData(out MatrixData SplitTo, int row, int col)
        {
            throw new NotImplementedException();
        }

        public void CopyData(out MatrixData CopyTo, int row, int col)
        {
            throw new NotImplementedException();
        }

        public void AddRow(object[] newRow)
        {
            if (newRow.Length != NumberOfColumns)
            {
                throw new Exception("Number of columns in the new row ("+newRow.Length+") must equal to the number of columns{"+NumberOfColumns+")");
            }
            object[,] newData = new object[NumberOfRows+1,NumberOfColumns];

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col <NumberOfColumns; col++)
                {
                    newData[row,col] = _data[row,col];
                }
            }
            for (int col = 0; col <NumberOfColumns; col++)
            {
                newData[NumberOfRows+1,col] = newRow[col];
            }
            _data = newData;
            NumberOfRows++;   
        }

        public void ChangeHeader(int col, string value)
        {
            _headers[col] = value;
        }

        //https://stackoverflow.com/questions/30164019/shuffling-2d-array-of-cards
        public void Suffle()
        {
            Random random = new Random();
            object[][] data = ToJagged(_data);
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
    }
}
