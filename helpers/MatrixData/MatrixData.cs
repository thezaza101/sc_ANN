using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace helpers
{
    public partial class MatrixData : ICloneable
    {
        //This represents the headers of the maxtrix information
        public string[] Headers { get => _headers; }
        public Type[] ColumnDataTypes {get => _columnDataTypes; }
        public object[,] Data {get => _data; }

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

        public MatrixData(MatrixData dt, int rowStart, int colStart, int numRows = 0, int numCols = 0)
        {
            _data = dt._data.Clone() as object[,];
            _columnDataTypes = dt._columnDataTypes.Clone() as Type[];
            _headers = dt._headers.Clone() as string[];

            this.NumberOfColumns = dt.NumberOfColumns;
            this.NumberOfRows = dt.NumberOfRows;
            this.DefaultNumericType = dt.DefaultNumericType;

            int rowsToKeep = (numRows == 0)? (NumberOfRows - rowStart) : numRows;
            int colsToKeep = (numCols == 0)? (NumberOfColumns - colStart) : numCols;

            TopSplit(rowStart, rowsToKeep);
            LeftSplit(colStart, colsToKeep);
        }

        public MatrixData(System.Data.DataTable dt)
        {

        }

        public MatrixData SplitData(int rowStart, int colStart, int numRows = 0, int numCols = 0)
        {
            MatrixData splitData = new MatrixData(this, rowStart, colStart, numRows, numCols);
            int rowsToKeep = (rowStart==0)? NumberOfRows : NumberOfRows-rowStart;
            int colsToKeep = (colStart==0)? NumberOfColumns: NumberOfColumns-colStart;;
            TopSplit(0,rowsToKeep);
            LeftSplit(0,colsToKeep);
            return splitData;
        }

        public MatrixData CopyData(int row, int col, int numRows = 0, int numCols = 0)
        {
            return new MatrixData(this, row, col, numRows, numCols);
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

        public void TopSplit(int rowStart, int numRowsToKeep)
        {
            object[,] newData = new object[numRowsToKeep,NumberOfColumns];

            for (int row = rowStart; row < rowStart+numRowsToKeep; row++) 
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    newData[row-rowStart, col] = _data[row, col];
                }
            }
            _data = newData;
            NumberOfRows = numRowsToKeep;
        }

        public void LeftSplit(int colStart, int numColsToKeep)
        {
            object[,] newData = new object[NumberOfRows,numColsToKeep];
            string[] newHeaders = new string[numColsToKeep];

            Type[] newColumnDataTypes = new Type[numColsToKeep];
            for (int col = colStart; col < colStart+numColsToKeep; col++)
            {
                newHeaders[col-colStart] = _headers[col];
                newColumnDataTypes[col-colStart] = _columnDataTypes[col];
            }
            _headers = newHeaders;
            _columnDataTypes = newColumnDataTypes;

            for (int row = 0; row < NumberOfRows; row++) 
            {
                for (int col = colStart; col < colStart+numColsToKeep; col++)
                {
                    newData[row, col-colStart] = _data[row, col];
                }
            }
            _data = newData;
            NumberOfColumns = _headers.Length;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
