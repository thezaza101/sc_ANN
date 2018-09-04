using System;
using System.Collections.Generic;
using System.Linq;
using helpers;
using NeuralNetworks;

namespace cli
{
    class Program
    {
        //private static readonly List<string> PossibleResults = new List<string> { "1","2","3" };
        static void Main(string[] args)
        {
            //Week 4
            //LoadData
            MatrixData irisRaw = new MatrixData("iris - Copy.Txt", false,true,' ');
            irisRaw.ChangeHeader(0,"Speal.Length");
            irisRaw.ChangeHeader(1,"Speal.Width");
            irisRaw.ChangeHeader(2,"Petal.Length");
            irisRaw.ChangeHeader(3,"Petal.Width");
            irisRaw.ChangeHeader(4,"Species");

            System.Console.WriteLine("Raw data:");
            System.Console.WriteLine(irisRaw.Head(5,16));
            System.Console.WriteLine();

            System.Console.WriteLine("Normalizeing columns 1:4...");
            irisRaw.Normalize(0);
            irisRaw.Normalize(1);
            irisRaw.Normalize(2);
            irisRaw.Normalize(3);
            
            System.Console.WriteLine("Normalised data:");
            System.Console.WriteLine(irisRaw.Head(5,16));
            System.Console.WriteLine();

            MatrixData irisExemplar = irisRaw.GetExemplar(4, 3, 1);
            System.Console.WriteLine("Exemplar data:");
            System.Console.WriteLine(irisExemplar.Head(5,16));
            System.Console.WriteLine();

            System.Console.WriteLine("Suffleing data...");
            irisExemplar.Suffle();
            System.Console.WriteLine();

            System.Console.WriteLine("Setting trainig data as first 50 rows of suffled exemplar data...");
            MatrixData trainData = irisExemplar.SplitData(0,0,50);
            System.Console.WriteLine("Trainig data:");
            System.Console.WriteLine(trainData.Head(5,16));
            System.Console.WriteLine();

            System.Console.WriteLine("Setting test data as next 50 rows of suffled exemplar data...");
            MatrixData testData =irisExemplar.SplitData(0,0,50);
            System.Console.WriteLine("Test data:");
            System.Console.WriteLine(testData.Head(5,16));
            System.Console.WriteLine();


            System.Console.WriteLine("Setting validation data as next 50 rows of suffled exemplar data...");
            MatrixData valData = irisExemplar.SplitData(0,0,50);
            System.Console.WriteLine("Validation data:");
            System.Console.WriteLine(valData.Head(5,16));
            System.Console.WriteLine();
            System.Console.WriteLine();


            int num_inputs = 4; // from iris data set
            int num_hidden = 1; // arbitary
            int num_outputs = 3;// from iris data set
            int epochs = 200; // For tute 3 
            double eta = 0.1;// learning_rate
            
            System.Console.WriteLine("Initialising Neural Network with:");
            System.Console.WriteLine("{0} inputs, {1} hidden layers, {2} outputs, {3} epochs, {4} learning eate", num_inputs, num_hidden, num_outputs, epochs,eta);

            Random rnd1 = new Random(102);
            W4NeuralNetwork nn = new W4NeuralNetwork(num_inputs, num_hidden, num_outputs, rnd1);
            nn.InitializeWeights(rnd1);

            string dir = "Data\\";
            nn.train(testData.Data.ToJagged().ToDoubleArray(), testData.Data.ToJagged().ToDoubleArray(), epochs, eta, dir+"nnlog.txt");

        
            double trainAcc = nn.Accuracy(trainData.Data.ToJagged().ToDoubleArray(),dir+"trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir+"trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.Data.ToJagged().ToDoubleArray(),dir+"testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.Data.ToJagged().ToDoubleArray(),dir+"valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");


            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            System.Console.WriteLine();

            System.Console.WriteLine("Train accuracy = " + trainAcc.ToString("F2"));
            System.Console.WriteLine("Test accuracy = " + testAcc.ToString("F2"));
            System.Console.WriteLine("Val accuracy = " + valAcc.ToString("F2"));
            System.Console.WriteLine();
            System.Console.WriteLine("Train Confusion matrix \r\n"+ConfusionTrain);
            System.Console.WriteLine("Test Confusion matrix \r\n"+ConfusionTest);
            System.Console.WriteLine("Val Confusion matrix \r\n"+ConfusionVal);
            








            /*MatrixData inputs = new MatrixData("iris - Copy.Txt", false,' ');
            inputs.Suffle();

            SNeuralNetwork network = new SNeuralNetwork(4, 5, 3, 0.2);

            MatrixData trainData = inputs.CopyData();
            trainData.TopSplit(0,100);
            int epochs = 500;

            Console.WriteLine($"Training network with {trainData.Length} samples using {epochs} epochs...");

            for (var epoch = 0; epoch < epochs; epoch++)
            {
                foreach (var input in trainData.Data.ToJagged())
                {
                    var targets = new[] { 0.01, 0.01, 0.01 };
                    targets[PossibleResults.IndexOf(input.Last().ToString())] = 0.99;

                    var inputList = input.Take(4).ToArray();
                    var inputData = new MatrixData(inputList);
                    inputData.NormalizeAll();
                    network.Train(inputData, new MatrixData(targets.ToDynamicArray()));
                }
            }

            
            /*MatrixData testData = new MatrixData("mnist_test.csv",false);
            System.Console.WriteLine(testData.Head(100,20));
            /*MatrixData data = new MatrixData("iris - Copy.Txt", false,' ');
            data.ChangeHeader(0,"Speal.Length");
            data.ChangeHeader(1,"Speal.Width");
            data.ChangeHeader(2,"Petal.Length");
            data.ChangeHeader(3,"Petal.Width");
            data.ChangeHeader(4,"Species");

            Console.WriteLine(data.Head(5,20));
           
            /*Console.WriteLine();
            Console.WriteLine("Normalizeing Data...");

            data.Normalize(0,NormalizationMethod.StandardScore);
            data.Normalize(1,NormalizationMethod.StandardScore);
            data.Normalize(2,NormalizationMethod.StandardScore);
            data.Normalize(3,NormalizationMethod.StandardScore);
            data.Suffle();
            System.Console.WriteLine(data.Head(150,20));

            Console.WriteLine();

            Console.WriteLine(data.Head(20,20));

            Console.WriteLine();
            Console.WriteLine("Sorting Data (0, acen)...");
            Console.WriteLine();
            
            data.Sort(0);
            Console.WriteLine(data.Head(5,20));

            Console.WriteLine();
            Console.WriteLine("Sorting Data (0, decn)...");
            Console.WriteLine();

            data.Sort(0,false);
            Console.WriteLine(data.Head(5,20));

            Console.WriteLine();
            Console.WriteLine("Suffleing data...");
            Console.WriteLine();

            data.Suffle();
            Console.WriteLine(data.Head(5,20));

            Console.WriteLine();
            Console.WriteLine("Sorting Data (0, decn)...");
            Console.WriteLine();

            data.Sort(0,false);
            Console.WriteLine(data.Head(5,20));

            data.WriteCSV("test.csv");

            MatrixData data = new MatrixData("test.csv", false);
            //System.Console.WriteLine(data.Head(10));

            //System.Console.WriteLine("CopyData(0,0) test");
            //MatrixData data2 = data.CopyData(0,0);
            //System.Console.WriteLine(data.Head(10));
            
            System.Console.WriteLine("Spitting the data in half..");
            MatrixData secondHalf = data.SplitData(5,0);
            System.Console.WriteLine(data.Head());
            System.Console.WriteLine(secondHalf.Head());
            MatrixData secondColHalf = data.SplitData(0,5);
            System.Console.WriteLine(data.Head());
            System.Console.WriteLine(secondColHalf.Head());*/

            /*MatrixData data = new MatrixData("task2z11.txt", false,' ');

            Console.WriteLine(data.Head(10,20));

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();

            MatrixData exemplarData = data.GetExemplar(2,2,1);
            Console.WriteLine(exemplarData.ToString());
            var test = data.IndexOf(0,data[1,0]);*/


            /*var matrix = new dynamic[][] {
                new dynamic[]{1,2,3},
                new dynamic[]{4,5,6},
                new dynamic[]{7,8,9}
            };

            MatrixData m = new MatrixData(matrix);
            var colNames = new string[]{"c1","c2","c3"};
            var rowNames = new string[]{"r1","r2","r3"};
            m.ChangeHeader(colNames);
            m.ChangeRowHeader(rowNames);
            m.LabeledRows = true;
            System.Console.WriteLine(m.Head(3));*/

        }
    }
}
