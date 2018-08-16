using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace helpers
{
    public partial class MatrixData<T>
    {
        //attempt to normalize a numaric column
        public void Normalize(int col, NormalizationMethod method = NormalizationMethod.FeatureScalingMinMax)
        {
            List<Type> numaricTypes = new List<Type>();
            numaricTypes.Add(typeof(double));
            numaricTypes.Add(typeof(int));
            numaricTypes.Add(typeof(decimal));
            if(numaricTypes.Contains(_data[0,col].GetType()))
            {

                //MinMax
                //https://docs.microsoft.com/en-us/azure/machine-learning/studio-module-reference/normalize-data
                //https://en.wikipedia.org/wiki/Normalization_(statistics)
                //https://en.wikipedia.org/wiki/Feature_scaling
                double min = double.Parse(_data[0,col].ToString());
                double max = double.Parse(_data[0,col].ToString());
                
                //find the minimum and maximum value in the column
                for (int row = 1; row < NumberOfRows; row++)
                {
                    double currentVal = double.Parse(_data[row,col].ToString());
                    min = (currentVal < min)? currentVal : min;
                    max = (currentVal > max)? currentVal : max;
                }

                double mult = 1 / (max - min);
                
                for (int row = 0; row < NumberOfRows; row++)
                {
                    double currentVal = double.Parse(_data[row,col].ToString());
                    currentVal = (currentVal - min) * mult;
                    _data[row,col] = Convert(currentVal.ToString());
                }
            }
        }
    }
}