using System;
using System.Collections.Generic;
namespace helpers
{
    public static class MatrixDataExtension
    {
        public static object Add(this object o, object valueToAdd)
        {
            if (o.IsValueNumaric())
            {
                return (double)o + (double)valueToAdd;
            }
            else
            {
                return o.ToString()+valueToAdd.ToString();
            }
        }
        public static object Subtract(this object o, object valueToSubrtact)
        {
            if (o.IsValueNumaric())
            {
                return (double)o - (double)valueToSubrtact;
            }
            else
            {
                throw new InvalidOperationException("Cannot add non numaric types");
            }
        }
        public static object DevideBy(this object o, object valueToDevideBy)
        {
            if (o.IsValueNumaric())
            {
                return (double)o / (double)valueToDevideBy;
            }
            else
            {
                throw new InvalidOperationException("Cannot devide by non numaric types");
            }
        }
        public static object MultiplyBy(this object o, object valueToDevideBy)
        {
            if (o.IsValueNumaric())
            {
                return (double)o * (double)valueToDevideBy;
            }
            else
            {
                throw new InvalidOperationException("Cannot multiply by non numaric types");
            }
        }

        public static bool IsValueNumaric(this object o)
        {
            double d;
            return double.TryParse(o.ToString(), out d);
        }


        public static MatrixData AddMatrix(this MatrixData data1, MatrixData data2)
        {
            throw new NotImplementedException();
        }

        public static MatrixData SubtractMatrix(this MatrixData data1, MatrixData data2)
        {
            throw new NotImplementedException();
        }

        public static MatrixData DevideMatrix(this MatrixData data1, MatrixData data2)
        {
            throw new NotImplementedException();
        }
        public static MatrixData MultiplyMatrix(this MatrixData data1, MatrixData data2)
        {
            throw new NotImplementedException();
        }



        public static double ToDouble(this object o)
        {
            double d;
            double.TryParse(o.ToString(),out d);
            return d;
        }
        public static double ToInt32(this object o)
        {
            int i;
            int.TryParse(o.ToString(), out i);
            return i;
        }

        //Convert rectangular array to jagged array
        public static object[][] ToJagged(this object[,] array)
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
        public static object[,] ToRectangular(this object[][] array)
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
        public static T[] GenerateEmptyArray<T>(int size,T value) 
        {
            T[] output = new T[size];

            for ( int i = 0; i < output.Length;i++ ) {
                output[i] = value;
            }
            return output;
        }
    }
}