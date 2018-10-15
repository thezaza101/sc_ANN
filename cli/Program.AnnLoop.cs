using System;
using System.Collections.Generic;
using System.Linq;
using helpers;
using NeuralNetworks;
using System.Threading;
using System.Threading.Tasks;

namespace cli
{
    class Program
    {
        private static MatrixData _rawData;
        private static MatrixData _exemplarData;
        private static MatrixData _trainingData;
        private static MatrixData _testingData;
        private static MatrixData _validationData;

        static void Main(string[] args)
        {
            //Run("Data\\Assignment1\\Dermatology\\","dermatology.csv",34,6,',',122,122,122,true,0,33,50,6,0.1,100);
            //Run("Data\\Assignment1\\Task2\\","task2_2018a.txt",2,2,',',168,166,166,false,0,1,750,5,0.1,100);
            //Run("Data\\Assignment1\\Task2\\","task2_2018b.txt",2,2,',',100,100,100,false,0,1,900,7,0.15,100);
            //Run("Data\\Assignment1\\Task2\\","task2_2018c.txt",2,2,',',134,133,133,false,0,1,700,5,0.1,100);
            Run("Data\\Assignment1\\Task4\\","FaceDataBoth.csv",49,6,',',2327,2325,2325,true,0,48,65,6,0.1,100);
            //Run("Data\\","Iris",4,3,' ',50,50,50,true,0,3,100,300,100,1,10,1,0.01,0.1,0.01);
            //Run("Data\\Cancer\\","cancer.txt",9,2,',',227,228,228,true,0,8,10,100,5,1,4,1,0.01,0.15,0.01);
            //Run("Data\\","abalone.txt",8,3,',',1392,1392,1392,true,0,7,100,3000,100,1,16,1,0.01,0.1,0.01);
            //Run("Data\\Card\\","card.txt",51,2,',',230,230,230,true,0,50,50,500,10,1,10,1,0.01,0.10,0.03);
            //Run("Data\\WeedSeed\\","weedseed.txt",7,10,',',134,132,132,true,0,6,200,500,50,2,10,2,0.01,0.1,0.01);

        }

        static void Run(string dir, string file, int inputs,
        int outputs,char dlim, int trainSplit, int testSplit, int valSplit,
        bool norm, int normStart, int normEnd, int Ep, int Hid, double Eta, int runs)
        {
            PrepareFile(dir+file,inputs,outputs,dlim,trainSplit,testSplit,valSplit,norm,normStart,normEnd);
            Random rnd1 = new Random(102);
            List<double[]> outputList = new List<double[]>();
            for (int i = 0; i<runs;i++)
            {
                suffleData((i*10)+100,trainSplit,testSplit,valSplit);
                double trainAcc = 0;
                double testAcc = 0;
                double valAcc = 0;
                MatrixData runResults = RunNetwork(inputs,Hid,outputs,Ep,Eta,rnd1, out trainAcc, out testAcc, out valAcc);
                double[] d = new double[7];
                d[0] = Ep;
                d[1] = Hid;
                d[2] = Eta;
                d[3] = (i*10)+100;
                d[4] = trainAcc;
                d[5] = testAcc;
                d[6] = valAcc;
                outputList.Add(d);
                System.Console.WriteLine(DateTime.Now.ToString() +" ("+ i.ToString()+"/"+runs.ToString()+"): "+Ep.ToString()+","+Hid.ToString()+","+Eta.ToString());
            }
            


           
            dynamic[,] outputListArray = ToRectangular(outputList.ToArray());
            
            MatrixData output = new MatrixData(outputListArray);
            output.ChangeHeader(0,"Epochs");
            output.ChangeHeader(1,"HiddenLayers");
            output.ChangeHeader(2,"eta");
            output.ChangeHeader(3,"Seed");

            output.ChangeHeader(4,"TrainAcc");
            output.ChangeHeader(5,"TestAcc");
            output.ChangeHeader(6,"ValAcc");


            string filename= dir+file+DateTime.Now.Ticks+"Out.csv";
            output.WriteCSV(filename);
            System.Console.WriteLine(output.Mean(4));            
            System.Console.WriteLine(output.Mean(6));
            System.Console.WriteLine(filename);


        }

        static void PrepareFile(string InputFile, int NumberInputsNodes, int NumberOutputNodes, char Delimiter, int train, int test, int val, bool norm, int normStart = 0, int normEnd = 3)
        {
            _rawData = new MatrixData(InputFile, false, true,Delimiter);
            foreach (var v in _rawData.Headers)
            {
                var x = _rawData.GetColumnSummary(_rawData.IndexOf(v));
                Console.WriteLine(x.ToString());
            }

            if(norm)
            {
                for (int i = normStart; i<=normEnd;i++)
                {
                    _rawData.Normalize(i);
                }
            }
            _exemplarData = _rawData.GetExemplar(NumberInputsNodes, NumberOutputNodes, 1);

            
        }

        static void suffleData(int seed, int train, int test, int val)
        {
            _exemplarData.Suffle(seed);

            _trainingData = _exemplarData.CopyData(0,0,train);

            _testingData =_exemplarData.CopyData(train,0,test);

            _validationData = _exemplarData.CopyData(train+test,0,val);
        }

        static MatrixData RunNetwork(int num_inputs, int num_hidden, int num_outputs, int epochs, double eta, Random r, out double tr, out double te, out double val)
        {
            W4NeuralNetworkTS nn = new W4NeuralNetworkTS(num_inputs, num_hidden, num_outputs, r);
            nn.InitializeWeights(r);
            string dir = "Data\\";
            nn.train(_trainingData.Data.ToJagged().ToDoubleArray(), _testingData.Data.ToJagged().ToDoubleArray(), epochs, eta, dir+"nnlog.txt");

            tr = nn.Accuracy(_trainingData.Data.ToJagged().ToDoubleArray(),dir+"");

            te = nn.Accuracy(_testingData.Data.ToJagged().ToDoubleArray(),dir+"");

            val =  nn.Accuracy(_validationData.Data.ToJagged().ToDoubleArray(),"");


            return nn.GraphData;
        }

        public static IEnumerable<int> SteppedIntegerList(int startIndex, int endEndex, int stepSize)
        {
            for (int i = startIndex; i < endEndex; i += stepSize)
            {
                yield return i;
            }
        }
        public static dynamic[,] ToRectangular(double[][] array)
        {
            int height = array.Length;
            int width = array[0].Length;
            dynamic[,] rect = new dynamic[height, width];
            for (int i = 0; i < height; i++)
            {
                dynamic[] row = array[i].ToDynamicArray();
                for (int j = 0; j < width; j++)
                {
                    rect[i, j] = row[j];
                }
            }
            return rect;
        }
    }
}
