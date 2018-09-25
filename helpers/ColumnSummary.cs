using System;
using System.Collections.Generic;


namespace helpers
{
    public class ColumnSummary
    {
        public string ColumnHeader {get;set;}
        public Type ColumnType {get;set;}
        public Range Range {get;set;}
        public Dictionary<string, int> UniqueValues {get;set;}

        public double Mean {get;set;}
        public double Mode {get;set;}
        public double Median {get;set;}



        public ColumnSummary(){}
        public ColumnSummary(string columnHeader,Type columnType, Range range, double mean, double mode, double median, Dictionary<string, int> uniqueValues)
        {
            this.ColumnHeader = columnHeader;
            this.ColumnType = columnType;
            this.Range = range;
            this.Mean = mean;
            this.Mode = mode;
            this.Median = median;
            this.UniqueValues = uniqueValues;
        }
    
        

        public override string ToString()
        {
            string output ="";
            output+= "Name: "+ColumnHeader + Environment.NewLine;
            output+= "Type: "+ColumnType.ToString() + Environment.NewLine;;
            if(ColumnType == typeof(double) | ColumnType == typeof(int) | ColumnType == typeof(decimal))
            {
                output+= "Min: "+Range.Min + Environment.NewLine;
                output+= "Mean: "+Mean.ToString("F2") + Environment.NewLine;
                output+= "Mode: "+Mode.ToString("F2") + Environment.NewLine;
                output+= "Median: "+Median.ToString("F2") + Environment.NewLine;
                output+= "Max: "+Range.Max + Environment.NewLine;
            }
            output+= "Unique: "+UniqueValues.Count + Environment.NewLine;
            if(ColumnType == typeof(string) & UniqueValues.Count <= 10)
            {
                output+= "Values: "+ Environment.NewLine;
                foreach (var v in UniqueValues)
                {
                    output+= "-"+v.Key+": "+v.Value + Environment.NewLine;
                }
            }
            return output;
        }
    }
}