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
            
            RunIris();
            //RunAbalone();
        }
        static void RunIris()
        {
            PrepareFile("Iris",4,3,' ',50,50,50,true,0,3);

            int MaxEpochs = 3000;
            int MaxHidden = 16;
            double MaxLearnRate = 0.1;

            int MinEpochs = 200;
            int MinHidden = 1;
            double MinLearnRate = 0.01;

            int StepEpochs = 100;
            int StepHidden = 1;
            double StepLearnRate = 0.01;


            int epx = ((MaxEpochs-MinEpochs)/StepEpochs)+1;
            int hidx = ((MaxHidden-MinHidden)/StepHidden)+1;
            double lrx = ((MaxLearnRate-MinLearnRate)/StepLearnRate)+1;

            int rows = int.Parse((epx * hidx * lrx).ToString());

            MatrixData output = new MatrixData(rows,11);
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

            Random rnd1 = new Random(102);
            int rowNum = 0;

            Parallel.ForEach(SteppedIntegerList(MinEpochs, MaxEpochs, StepEpochs), ep => { 
                for (int hid = MinHidden; hid<MaxHidden; hid+=StepHidden)
                {
                    for (double eta = MinLearnRate; eta<MaxLearnRate; eta+=StepLearnRate)
                    {
                        MatrixData runResults = RunNetwork(4,hid,3,ep,eta,rnd1);
                        output[rowNum,0] = ep;
                        output[rowNum,1] = hid;
                        output[rowNum,2] = eta;
                        output[rowNum,3] = runResults.Max(1);
                        output[rowNum,4] = runResults.Max(2);
                        output[rowNum,5] = runResults.Mean(1);
                        output[rowNum,6] = runResults.Mean(2);
                        output[rowNum,7] = runResults.Mode(1);
                        output[rowNum,8] = runResults.Mode(2);
                        output[rowNum,9] = runResults.Median(1);
                        output[rowNum,10] = runResults.Median(2);
                        System.Console.WriteLine(DateTime.Now.ToString() +" ("+ rowNum.ToString()+"/"+rows.ToString()+"): "+ep.ToString()+","+hid.ToString()+","+eta.ToString());
                        rowNum++;
                    }
                }} );
             
            output.TopSplit(0,rowNum);

            MatrixData m = new MatrixData("abaloneOut.csv",true,true,',');
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

            //output.NormalizeAll();
            output.WriteCSV("IrisOut.csv");
        }
        static void RunAbalone()
        {
            PrepareFile("abalone.txt",8,3,',',1392,1392,1392,true,0,7);

            int MaxEpochs = 3000;
            int MaxHidden = 16;
            double MaxLearnRate = 0.1;

            int MinEpochs = 200;
            int MinHidden = 1;
            double MinLearnRate = 0.01;

            int StepEpochs = 100;
            int StepHidden = 1;
            double StepLearnRate = 0.01;


            int epx = ((MaxEpochs-MinEpochs)/StepEpochs)+1;
            int hidx = ((MaxHidden-MinHidden)/StepHidden)+1;
            double lrx = ((MaxLearnRate-MinLearnRate)/StepLearnRate)+1;

            int rows = int.Parse((epx * hidx * lrx).ToString());

            MatrixData output = new MatrixData(rows,11);
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

            Random rnd1 = new Random(102);
            int rowNum = 0;

            Parallel.ForEach(SteppedIntegerList(MinEpochs, MaxEpochs, StepEpochs), ep => { 
                for (int hid = MinHidden; hid<MaxHidden; hid+=StepHidden)
                {
                    for (double eta = MinLearnRate; eta<MaxLearnRate; eta+=StepLearnRate)
                    {
                        MatrixData runResults = RunNetwork(8,hid,3,ep,eta,rnd1);
                        output[rowNum,0] = ep;
                        output[rowNum,1] = hid;
                        output[rowNum,2] = eta;
                        output[rowNum,3] = runResults.Max(1);
                        output[rowNum,4] = runResults.Max(2);
                        output[rowNum,5] = runResults.Mean(1);
                        output[rowNum,6] = runResults.Mean(2);
                        output[rowNum,7] = runResults.Mode(1);
                        output[rowNum,8] = runResults.Mode(2);
                        output[rowNum,9] = runResults.Median(1);
                        output[rowNum,10] = runResults.Median(2);
                        System.Console.WriteLine(DateTime.Now.ToString() +"("+ rowNum.ToString()+"/"+rows.ToString()+"): "+ep.ToString()+","+hid.ToString()+","+eta.ToString());
                        rowNum++;
                    }
                }} );
             
            output.TopSplit(0,rowNum);

            MatrixData m = new MatrixData("abaloneOut.csv",true,true,',');
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

            //output.NormalizeAll();
            output.WriteCSV("abaloneOut.csv");
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

        static MatrixData RunNetwork(int num_inputs, int num_hidden, int num_outputs, int epochs, double eta, Random r)
        {
            W4NeuralNetworkTS nn = new W4NeuralNetworkTS(num_inputs, num_hidden, num_outputs, r);
            nn.InitializeWeights(r);
            string dir = "Data\\";
            nn.train(_trainingData.Data.ToJagged().ToDoubleArray(), _testingData.Data.ToJagged().ToDoubleArray(), epochs, eta, dir+"nnlog.txt");
            return nn.GraphData;
        }

        public static IEnumerable<int> SteppedIntegerList(int startIndex, int endEndex, int stepSize)
        {
            for (int i = startIndex; i < endEndex; i += stepSize)
            {
                yield return i;
            }
        }
    }
}
