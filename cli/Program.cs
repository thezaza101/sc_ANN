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
            //Run("Data\\","Iris",4,3,' ',50,50,50,true,0,3,100,300,100,1,10,1,0.01,0.1,0.01);
            //Run("Data\\","cancer.txt",9,2,',',227,228,228,true,0,8,10,100,5,1,4,1,0.01,0.15,0.01);
            //Run("Data\\","abalone.txt",8,3,',',1392,1392,1392,true,0,7,100,3000,100,1,16,1,0.01,0.1,0.01);
            Run("Data\\","card.txt",51,2,',',230,230,230,true,0,50,50,500,10,1,10,1,0.01,0.10,0.03);
        }

        static void Run(string dir, string file, int inputs,
        int outputs,char dlim, int trainSplit, int testSplit, int valSplit,
        bool norm, int normStart, int normEnd, int MinEp, int MaxEp,int StepEp,
        int MinHid, int MaxHid,int StepHid,double MinEta, double MaxEta,double StepEta)
        {
            PrepareFile(dir+file,inputs,outputs,dlim,trainSplit,testSplit,valSplit,norm,normStart,normEnd);

            int MaxEpochs = MaxEp;
            int MaxHidden = MaxHid;
            double MaxLearnRate = MaxEta;

            int MinEpochs = MinEp;
            int MinHidden = MinHid;
            double MinLearnRate = MinEta;

            int StepEpochs = StepEp;
            int StepHidden = StepHid;
            double StepLearnRate = StepEta;

            int epx = ((MaxEpochs-MinEpochs)/StepEpochs)+1;
            int hidx = ((MaxHidden-MinHidden)/StepHidden)+1;
            double lrx = ((MaxLearnRate-MinLearnRate)/StepLearnRate)+1;

            int rows = int.Parse((epx * hidx * lrx).ToString());

            Random rnd1 = new Random(102);
            int rowNum = 0;
            List<double[]> outputList = new List<double[]>();
            Parallel.ForEach(SteppedIntegerList(MinEpochs, MaxEpochs, StepEpochs), ep => { 
                for (int hid = MinHidden; hid<MaxHidden; hid+=StepHidden)
                {
                    for (double eta = MinLearnRate; eta<MaxLearnRate; eta+=StepLearnRate)
                    {
                        double trainAcc = 0;
                        double testAcc = 0;
                        double valAcc = 0;
                        MatrixData runResults = RunNetwork(inputs,hid,outputs,ep,eta,rnd1, out trainAcc, out testAcc, out valAcc);
                        double[] d = new double[14+MaxEp-MinEp];
                        d[0] = ep;
                        d[1] = hid;
                        d[2] = eta;
                        d[3] = runResults.Max(1);;
                        d[4] = runResults.Max(2);;
                        d[5] = runResults.Mean(1);;
                        d[6] = runResults.Mean(2);;
                        d[7] = runResults.Mode(1);;
                        d[8] = runResults.Mode(1);;
                        d[9] = runResults.Median(1);;
                        d[10] = runResults.Median(2);;
                        d[11] = trainAcc;
                        d[12] = testAcc;
                        d[13] = valAcc;
                        outputList.Add(d);
                        System.Console.WriteLine(DateTime.Now.ToString() +" ("+ rowNum.ToString()+"/"+rows.ToString()+"): "+ep.ToString()+","+hid.ToString()+","+eta.ToString());
                        rowNum++;
                    }
                }} );
             
            dynamic[,] outputListArray = ToRectangular(outputList.ToArray());
            
            MatrixData output = new MatrixData(outputListArray);
            output.ChangeHeader(0,"Epochs");
            output.ChangeHeader(1,"HiddenLayers");
            output.ChangeHeader(2,"eta");
            output.ChangeHeader(3,"MaxTrain");
            output.ChangeHeader(4,"MaxTest");
            output.ChangeHeader(5,"MeanTrain");
            output.ChangeHeader(6,"MeanTest");
            output.ChangeHeader(7,"ModeTrain");
            output.ChangeHeader(8,"ModeTest");
            output.ChangeHeader(9,"MeanTrain");
            output.ChangeHeader(10,"MedianTest");
            output.ChangeHeader(11,"TrainAcc");
            output.ChangeHeader(12,"TestAcc");
            output.ChangeHeader(13,"ValAcc");

            MatrixData mCopy = new MatrixData(output,0,0);
            mCopy.Normalize(3);
            mCopy.Normalize(4);
            mCopy.Normalize(5);
            mCopy.Normalize(6);
            mCopy.Normalize(7);
            mCopy.Normalize(8);
            mCopy.Normalize(9);
            mCopy.Normalize(10);

            output.AddColumn(mCopy.Columns(3),"MaxTrainNorm");
            output.AddColumn(mCopy.Columns(4),"MaxTestNorm");
            output.AddColumn(mCopy.Columns(5),"MeanTrainNorm");
            output.AddColumn(mCopy.Columns(6),"MeanTestNorm");

            output.AddColumn(mCopy.Columns(7),"ModeTrainNorm");
            output.AddColumn(mCopy.Columns(8),"ModeTestNorm");
            output.AddColumn(mCopy.Columns(9),"MedianTrainNorm");
            output.AddColumn(mCopy.Columns(10),"MedianTestNorm");

            output.WriteCSV(dir+file+"Out.csv");

        }

        static void PrepareFile(string InputFile, int NumberInputsNodes, int NumberOutputNodes, char Delimiter, int train, int test, int val, bool norm, int normStart = 0, int normEnd = 3)
        {
            _rawData = new MatrixData(InputFile, false, true,Delimiter);

            if(norm)
            {
                for (int i = normStart; i<=normStart;i++)
                {
                    _rawData.Normalize(i);
                }
            }
            _exemplarData = _rawData.GetExemplar(NumberInputsNodes, NumberOutputNodes, 1);

            _exemplarData.Suffle();

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
