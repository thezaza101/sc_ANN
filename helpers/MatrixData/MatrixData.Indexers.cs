using System;
using System.Collections;

namespace helpers
{
    public partial class MatrixData
    {
        public int Length
        {
            get{
                return _data.Length;
            }
        }
        public object[] this[int row]
        {
            get
            {
                return _data.ToJagged()[row];
            }
        }
        public object this[int row, int col]
        {
            get
            {
                return _data[row,col];
            }
            set{
                _data[row,col] = value;
            }
        }
        public object[] Rows(int row)
        {
            return this[row];
        }

        public object[] Columns(string colName)
        {
            return Columns(Array.FindIndex(_headers,c => c.Equals(colName)));
        }
        public object[] Columns(int col)
        {
            object[] output = new object[NumberOfRows];
            for (int row = 0; row<NumberOfRows;row++)
            {
                output[row] = _data[row,col];
            }
            return output;
        }

        public T[] GetColumnCopy<T>(string colName)
        {
            return Columns(Array.FindIndex(_headers,c => c.Equals(colName))).Clone() as T[];
        }
        public T[] GetColumnCopy<T>(int col)
        {
            return Columns(col).Clone() as T[];
        }
        public MatrixData GetColumnCopy(string colName)
        {
            return GetColumnCopy(Array.FindIndex(_headers,c => c.Equals(colName)));
        }
        public MatrixData GetColumnCopy(int col)
        {
            return this.CopyData(0,col,0,1);
        }

        public int IndexOf(int col, object o)
        {
            return Array.IndexOf(Columns(col),o);
        }
    }
}