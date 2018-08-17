using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace helpers
{
    public partial class MatrixData<T>
    {
        //attempt to normalize a numaric column
        public void Normalize(int col, NormalizationMethod method = NormalizationMethod.StandardScore)
        {
            //https://docs.microsoft.com/en-us/azure/machine-learning/studio-module-reference/normalize-data
            //https://en.wikipedia.org/wiki/Normalization_(statistics)
            //https://en.wikipedia.org/wiki/Feature_scaling
            if(IsValueNumaric(_data[0,col]))
            {
                switch (method)
                {
                    case NormalizationMethod.StandardScore:
                        PreformStandardScoreNormalization(col);
                        break;
                    case NormalizationMethod.FeatureScalingStandardization:
                        PreformStandardization(col);
                        break;
                    case NormalizationMethod.FeatureScalingMinMax:
                        PreformMinMaxNormalization(col);
                        break;
                    case NormalizationMethod.FeatureScalingMean:
                        PreformMeanNormalization(col);
                        break;  
                    default:
                        throw new Exception("How did you even get here? Please let the monkey that"+
                        "coded this know the following: \"MatrixData<T>.Normalize.default\", along with"+
                        "what you did to cause this error");
                }
                
            }
        }
        private void PreformStandardScoreNormalization(int col)
        {
                //https://en.wikipedia.org/wiki/Standard_score
                double min = Min(col);
                double max = Max(col);
                
                double mult = 1 / (max - min);
                
                for (int row = 0; row < NumberOfRows; row++)
                {
                    double currentVal = double.Parse(_data[row,col].ToString());
                    currentVal = (currentVal - min) * mult;
                    _data[row,col] = Convert(currentVal.ToString());
                }
        }

        private void PreformMinMaxNormalization(int col)
        {
            //https://en.wikipedia.org/wiki/Feature_scaling
            double min = Min(col);
            double max = Max(col);
            for (int row = 0; row < NumberOfRows; row++)
            {
                double currentVal = double.Parse(_data[row,col].ToString());
                currentVal = (currentVal - min) / (max - min);
                _data[row,col] = Convert(currentVal.ToString());
            }
        }

        private void PreformMeanNormalization(int col)
        {
            //https://en.wikipedia.org/wiki/Feature_scaling
            double min = Min(col);
            double max = Max(col);
            double mean = Mean(col);

            for (int row = 0; row < NumberOfRows; row++)
            {
                double currentVal = double.Parse(_data[row,col].ToString());
                currentVal = (currentVal - mean) / (max - min);
                _data[row,col] = Convert(currentVal.ToString());
            }
        }

        private void PreformStandardization(int col)
        {
            //https://en.wikipedia.org/wiki/Feature_scaling
            PreformStandardScoreNormalization(col);
        }
        //Finds the minimum value for the given column
        public double Min(int col)
        {
            if(IsValueNumaric(_data[0,col]))
            {
                double min = double.Parse(_data[0,col].ToString());            
                //find the minimum value in the column
                for (int row = 1; row < NumberOfRows; row++)
                {
                    double currentVal = double.Parse(_data[row,col].ToString());
                    min = (currentVal < min)? currentVal : min;
                }
                return min;
            }
            else 
            {
                throw new InvalidOperationException("Cannot find minimum of non numaric value");
            }
        }
        //Finds the maximum value for the given column
        public double Max(int col)
        {
            if(IsValueNumaric(_data[0,col]))
            {
                double max = double.Parse(_data[0,col].ToString());            
                //find the maximum value in the column
                for (int row = 1; row < NumberOfRows; row++)
                {
                    double currentVal = double.Parse(_data[row,col].ToString());
                    max = (currentVal > max)? currentVal : max;
                }
                return max;
            }
            else 
            {
                throw new InvalidOperationException("Cannot find maximum of non numaric value");
            }

        }
        //Find the Mean (average) value for the given column
        public double Mean(int col)
        {
            if(IsValueNumaric(_data[0,col]))
            {
                return Sum(col) / NumberOfRows;
            }
            else 
            {
                throw new InvalidOperationException("Cannot find minimum of non numaric value");
            }
        }
        //Find the sum of a column
        public double Sum (int col)
        {
            if(IsValueNumaric(_data[0,col]))
            {
                double sum = 0;
                for (int row = 0; row < NumberOfRows; row++)
                {
                    sum+= double.Parse(_data[row,col].ToString());
                }
                return sum;
            }
            else 
            {
                throw new InvalidOperationException("Cannot find minimum of non numaric value");
            }
        }
        //Determines if the input value is numaric
        private bool IsValueNumaric(T value)
        {
            List<Type> numaricTypes = new List<Type>();
            numaricTypes.Add(typeof(double));
            numaricTypes.Add(typeof(int));
            numaricTypes.Add(typeof(decimal));
            return numaricTypes.Contains(value.GetType());
        }
    }
}