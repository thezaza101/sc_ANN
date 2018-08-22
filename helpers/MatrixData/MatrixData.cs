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
        public MatrixData(int numRows, int numCols, Type defaultNumericType = null)
        {
            SetDefaultNumericType(defaultNumericType);
            _headers = MatrixDataExtension.GenerateEmptyArray<string>(numCols,"");
            _columnDataTypes = MatrixDataExtension.GenerateEmptyArray<Type>(numCols,typeof(object));
            _data = new object[numRows,numCols];
            NumberOfRows = numRows;
            NumberOfColumns = numCols;
        }
        public MatrixData(string filelocation, bool hasHeaders = true, char delimiter = ',', Type defaultNumericType = null)
        {
            SetDefaultNumericType(defaultNumericType);
            ReadFromCSV(filelocation, hasHeaders,delimiter);
        }

        public MatrixData(MatrixData dt, int rowStart, int colStart, int numRows = 0, int numCols = 0, Type defaultNumericType = null)
        {
            SetDefaultNumericType(defaultNumericType);
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

        public MatrixData(System.Data.DataTable dt, Type defaultNumericType = null)
        {
            SetDefaultNumericType(defaultNumericType);
            int numRows = dt.Rows.Count;
            int numCols = dt.Columns.Count;
            _headers = MatrixDataExtension.GenerateEmptyArray<string>(numCols,"");
            _columnDataTypes = MatrixDataExtension.GenerateEmptyArray<Type>(numCols,typeof(object));
            _data = new object[numRows,numCols];
            NumberOfRows = numRows;
            NumberOfColumns = numCols;
            object[][] data = new object[numRows][];
            object[] row;
            for(int r = 0; r<numRows; r++)
            {
                row = new object[numCols];
                for (int c = 0; c<numCols; c++)
                {
                    row[c] = dt.Rows[r][c];
                }
                data[r] = row;
            }
            _data = data.ToRectangular();
            for (int c = 0; c<numCols; c++)
            {
                _headers[c] = dt.Columns[c].ColumnName;
            }            
            DetermineColTypes();
        }           

        private void SetDefaultNumericType(Type defaultNumericType)
        {
            if(!(defaultNumericType==null))
            {
                DefaultNumericType = defaultNumericType;
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
