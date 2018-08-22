using System;

namespace helpers
{
    public partial class MatrixData
    {
        public static MatrixData operator +(MatrixData a, MatrixData b)
        {
            if(a.Length != b.Length)
            {
                throw new InvalidOperationException("Matrices of different length cannot be added");
            }
            MatrixData output = a.CopyData(0,0);
            for(int r = 0;r<a.NumberOfRows;r++)
            {
                for(int c = 0;c<a.NumberOfColumns; c++)
                {
                    output[r,c] = output[r,c].Add(b[r,c]);
                }
            }
            return output;
        }
        public static MatrixData operator -(MatrixData a, MatrixData b)
        {
            if(a.Length != b.Length)
            {
                throw new InvalidOperationException("Matrices of different length cannot be subtracted");
            }
            MatrixData output = a.CopyData(0,0);
            for(int r = 0;r<a.NumberOfRows;r++)
            {
                for(int c = 0;c<a.NumberOfColumns; c++)
                {
                    output[r,c] = output[r,c].Subtract(b[r,c]);
                }
            }
            return output;
        }

        

        public static MatrixData operator +(MatrixData a, double b)
        {
            MatrixData output = a.CopyData(0,0);
            for(int r = 0;r<a.NumberOfRows;r++)
            {
                for(int c = 0;c<a.NumberOfColumns; c++)
                {
                    output[r,c] = output[r,c].Add(b);
                }
            }
            return output;
        }
        public static MatrixData operator -(MatrixData a, double b)
        {
            MatrixData output = a.CopyData(0,0);
            for(int r = 0;r<a.NumberOfRows;r++)
            {
                for(int c = 0;c<a.NumberOfColumns; c++)
                {
                    output[r,c] = output[r,c].Subtract(b);
                }
            }
            return output;
        }

        

    }
}