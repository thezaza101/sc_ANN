using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using helpers;

namespace nn
{
    public class SNeuralNetwork
    {
        private readonly double _learningRate;
        private MatrixData _weightHiddenOutput;
        private MatrixData _weightInputHidden;

        public SNeuralNetwork(int numberOfInputNodes, int numberOfHiddenNodes, int numberOfOutputNodes, double learningRate)
        {
            _learningRate = learningRate;

            _weightInputHidden = new MatrixData(numberOfHiddenNodes, numberOfInputNodes);
            _weightHiddenOutput = new MatrixData(numberOfOutputNodes, numberOfHiddenNodes);

            RandomizeWeights();
        }

        private void RandomizeWeights()
        {
            var rnd = new Random();
            //distribute -0.5 to 0.5.
            _weightHiddenOutput.SetAll(() => rnd.NextDouble() - 0.5);
            _weightInputHidden.SetAll(() => rnd.NextDouble() - 0.5);
        }

        public void Train(MatrixData inputs, MatrixData targets)
        {
            var inputSignals = inputs;
            var targetSignals = targets;

            var hiddenOutputs = Sigmoid(_weightInputHidden * inputs);
            var finalOutputs = Sigmoid(_weightHiddenOutput * hiddenOutputs);

            var outputErrors = targets - finalOutputs;
            
            var hiddenErrors = _weightHiddenOutput.GetTranspose() * outputErrors;

            _weightHiddenOutput += outputErrors * _learningRate * finalOutputs * (finalOutputs - 1.0) * hiddenOutputs.GetTranspose();
            _weightInputHidden += hiddenErrors * _learningRate * hiddenOutputs * (hiddenOutputs - 1.0) * inputs.GetTranspose();
        }
       
        public double[] Query(double[] inputs)
        {
            var inputSignals = ConvertToMatrix(inputs);
            var hiddenOutputs = Sigmoid(_weightInputHidden * inputSignals);
            var finalOutputs = Sigmoid(_weightHiddenOutput * hiddenOutputs);
            return Array.ConvertAll<dynamic,double>(finalOutputs.Data.ToJagged().SelectMany(x => x.Select(y => y)).ToArray() ,x=>(double)x);
        }

        private static MatrixData ConvertToMatrix(double[] inputList)
        {
            var input = new double[inputList.Length];

            for (var x = 0; x < input.Length; x++)
            {
                input[x] = inputList[x];
            }

            return new MatrixData(input.ToDynamicArray());
            //return new MatrixData(input,null);
        }

        private MatrixData Sigmoid(MatrixData matrix)
        {
            var newMatrix = new  MatrixData(matrix.Length, matrix.NumberOfColumns);

            for (var x = 0; x < matrix.Length; x++)
            {
                for (var y = 0; y < matrix.NumberOfColumns; y++)
                {
                    newMatrix[x,y] = 1 / (1 + Math.Pow(Math.E, -matrix[x,y]));
                }
            }
            return newMatrix;
        }
    }
}