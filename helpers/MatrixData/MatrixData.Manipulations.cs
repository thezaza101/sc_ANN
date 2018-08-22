using System;
using System.Linq;

namespace helpers
{
    public partial class MatrixData
    {
        public MatrixData SplitData(int rowStart, int colStart, int numRows = 0, int numCols = 0)
        {
            //This method will split the data based on the input and return it
            //it will also remove the data that was split from _data
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
        public void ChangeRow(int row, object[] newRow)
        {
            if (newRow.Length != NumberOfColumns)
            {
                throw new Exception("Number of columns in the new row ("+newRow.Length+") must equal to the number of columns{"+NumberOfColumns+")");
            }
            object[][] data = _data.ToJagged();
            data[row] = newRow;
            _data = data.ToRectangular();
        }
        public void ChangeHeader(int col, string value)
        {
            _headers[col] = value;
        }

        //https://stackoverflow.com/questions/30164019/shuffling-2d-array-of-cards
        public void Suffle()
        {
            Random random = new Random();
            object[][] data = _data.ToJagged();
            data = data.OrderBy(t => random.Next()).ToArray();
            _data = data.ToRectangular();
        }
        public void Transpose()
        {
            object[,] output = new object[NumberOfRows, NumberOfColumns];

            for (int i = 0; i < NumberOfColumns; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    output[j, i] = _data[i, j];
                }
            }
            _data = output;
        }

        public MatrixData GetExemplar(int col, int numClasses, int startAt = 0, string colName = "")
        {
            int cols = NumberOfColumns + numClasses - 1;            
            MatrixData exemplarData = new MatrixData(NumberOfRows,cols);            
            for (int r = 0; r < NumberOfRows; r++)
            {
                double[] rr = new double[cols];
                int cc = 0;
                for (int c = 0; c < NumberOfColumns; c++)
                {
                    double d = _data[r,c].ToDouble();
                    if (c == col)
                    {
                        for (int j=0; j<numClasses; j++)
                        {
                            rr[cc] = 0;
                            if ((j)==((int)d) - startAt)
                            {
                                rr[cc] = 1;
                            }
                            cc++;
                        }
                    }
                    else
                    {
                        rr[cc] = d;
                        cc++;
                    }
                }
                exemplarData.ChangeRow(r, Array.ConvertAll<double,object>(rr, x=> (object)x));
                //exemplarData.ReSetColTypes();
            }
            exemplarData.CopyMetaData(this);
            exemplarData.DetermineColType(cols-1);

            string headerName = (string.IsNullOrWhiteSpace(colName))? "Exemplar(" + col+","+ numClasses+"," + startAt+")" : colName;
            exemplarData.ChangeHeader(cols-1, headerName);
            return exemplarData;
        }

        public void Sort(int col, bool acen = true)
        {
            if(acen)
            {
                _data = _data.ToJagged().OrderBy(t => t[col]).ToArray().ToRectangular();
            }
            else
            {
                 _data = _data.ToJagged().OrderByDescending(t => t[col]).ToArray().ToRectangular();
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
    }
}