using System;
using System.Linq;
using System.Collections.Generic;

namespace helpers
{
    public partial class MatrixData
    {
        //Finds the minimum value for the given column
        public double Min(int col)
        {
            return GetColumnCopy<double>(col).Min();
        }

        
        //Finds the maximum value for the given column
        public double Max(int col)
        {
            return GetColumnCopy<double>(col).Max();
        }

        //Find the Mean (average) value for the given column
        public double Mean(int col)
        {
            return GetColumnCopy<double>(col).Average();
        }

        //find the Mode of a column
        public double Mode(int col)
        {
            return GetColumnCopy<double>(col)
                .GroupBy(v => v)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
        }

        //Find the Median of a column 
        public double Median(int col)
        {
            double[] column = GetColumnCopy<double>(col)
                .OrderByDescending(g => g).ToArray();
            int mid = NumberOfRows / 2;
            return (NumberOfRows % 2 != 0) ? (double)column[mid] : ((double)column[mid] + (double)column[mid - 1]) / 2;
        }

        //Find the sum of a column
        public double Sum(int col)
        {
            return GetColumnCopy<double>(col).Sum();
        }

        public int CountIf(int col, Func<dynamic, dynamic, bool> condition, dynamic value)
        {
            dynamic[] colData = Columns(col);
            int counter = 0;
            foreach (dynamic d in colData)
            {
                if(condition(value, d)) counter++;
            }
            return counter;
        }
        
        // Returns the range of a columns
        public Range ColRange(int col)
        {
            return new Range(this.Min(col),this.Max(col));
        }

        public Dictionary<string, int> UniqueValues(int col)
        {
            Dictionary<string, int> output = new Dictionary<string, int>();
            dynamic[] colval = Columns(col);
            dynamic uniqueVals = colval.Distinct();
            foreach (var v in uniqueVals)
            {                
                output.Add(Convert.ToString(v),colval.Count(c => object.Equals(c,v)));
            }
            return output;
        }

        public ColumnSummary GetColumnSummary(int col)
        {
            return new ColumnSummary(_headers[col],_columnDataTypes[col],ColRange(col),Mean(col),Mode(col),Median(col),UniqueValues(col));
            throw new System.Exception();
        }
    
    }
}