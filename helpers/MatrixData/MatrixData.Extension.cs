using System;
using System.Collections.Generic;
namespace helpers
{
    public static class MatrixDataExtension
    {
        public static dynamic Add(this object o, dynamic valueToAdd)
        {
            if (o.IsValueNumaric())
            {
                return (double)o + valueToAdd;
            }
            else
            {
                return o.ToString()+valueToAdd.ToString();
            }
        }
        public static object Subtract(this object o, dynamic valueToSubrtact)
        {
            if (o.IsValueNumaric())
            {
                return (double)o - valueToSubrtact;
            }
            else
            {
                throw new InvalidOperationException("Cannot add non numaric types");
            }
        }
        public static object DevideBy(this object o, dynamic valueToDevideBy)
        {
            if (o.IsValueNumaric())
            {
                return (double)o / valueToDevideBy;
            }
            else
            {
                throw new InvalidOperationException("Cannot devide by non numaric types");
            }
        }
        public static object MultiplyBy(this object o, dynamic valueToDevideBy)
        {
            if (o.IsValueNumaric())
            {
                return (double)o * valueToDevideBy;
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
        public static dynamic[][] ToJagged(this dynamic[,] array)
        {
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            dynamic[][] jagged = new dynamic[height][];

            for (int i = 0; i < height; i++)
            {
                dynamic[] row = new dynamic[width];
                for (int j = 0; j < width; j++)
                {
                    row[j] = array[i, j];
                }
                jagged[i] = row;
            }
            return jagged;
        }

        //Convert jagged array to rectangular array
        public static dynamic[,] ToRectangular(this dynamic[][] array)
        {
            int height = array.Length;
            int width = array[0].Length;
            dynamic[,] rect = new dynamic[height, width];
            for (int i = 0; i < height; i++)
            {
                dynamic[] row = array[i];
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
        public static dynamic[] ToDynamicArray(this double[] input)
        {
            return Array.ConvertAll<double, dynamic>(input, x=> (dynamic)x);
        }
        public static dynamic[][] ToDynamicArray(this double[][] input)
        {
            dynamic[][] output = new dynamic[input.Length][];
            for (int row = 0; row < input.Length;row++)
            {
                output[row] = Array.ConvertAll<double, dynamic>(input[row], x=> (dynamic)x);
            }
            return output;
        }
        public static dynamic[,] ToDynamicArray(this double[,] input)
        {
            dynamic[,] output = new dynamic[input.GetLength(0),input.GetLength(1)];
            for (int row = 0; row < input.GetLength(0);row++)
            {
                for (int col = 0; col< input.GetLength(1);col++)
                {
                    output[row,col] = input[row,col];
                }                
            }
            return output;
        }
        public static double[][] ToDoubleArray(this dynamic[][] input)
        {
            double[][] output = new double[input.Length][];
            for (int row = 0; row < input.Length;row++)
            {
                output[row] = Array.ConvertAll<dynamic, double>(input[row], x=> (double)x);
            }
            return output;
        }

        public static MatrixData GetTranspose(this MatrixData input)
        {
            MatrixData output = input.CopyData();
            output.Transpose();
            return output;
        }
        public static string[] Lines(this string input) => input.Split(new [] { '\r', '\n' });
        
        
    }
}