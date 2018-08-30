using System;
using helpers;
using nn;
using PLplot;

namespace ui
{
    public class ANN
    {
        public MatrixData RawData {get{return (_rawData==null)? new MatrixData(5,5) : _rawData;}}
        public MatrixData ExemplarData {get{return (_exemplarData==null)? new MatrixData(5,5) : _exemplarData;}}
        public MatrixData TrainingData {get{return (_trainingData==null)? new MatrixData(5,5) : _trainingData;}}
        public MatrixData TestingData {get{return (_testingData==null)? new MatrixData(5,5) : _testingData;}}
        public MatrixData ValidationData {get{return (_validationData==null)? new MatrixData(5,5) : _validationData;}}

        public MatrixData ConfusionMatrixTrain {get{return (_confusionMatrixTrain==null)? new MatrixData(5,5) : _confusionMatrixTrain;}}
        public MatrixData ConfusionMatrixTest {get{return (_confusionMatrixTest==null)? new MatrixData(5,5) : _confusionMatrixTest;}}
        public MatrixData ConfusionMatrixVal {get{return (_confusionMatrixVal==null)? new MatrixData(5,5) : _confusionMatrixVal;}}

        public MatrixData OutputMatrixTrain {get{return (_outputMatrixTrain==null)? new MatrixData(5,5) : _outputMatrixTrain;}}
        public MatrixData OutputMatrixTest {get{return (_outputMatrixTest==null)? new MatrixData(5,5) : _outputMatrixTest;}}
        public MatrixData OutputMatrixVal {get{return (_outputMatrixVal==null)? new MatrixData(5,5) : _outputMatrixVal;}}
        public MatrixData GraphData {get{return (_graphData==null)? new MatrixData(5,2) : _graphData;}}


        public int NumberInputsNodes {get;set;} = 4; // from iris data set
        public int NumberHiddenNodes {get;set;} = 1; // arbitary
        public int NumberOutputNodes {get;set;} = 3;// from iris data set
        public int NumberOfEpochs {get;set;} = 200; // For tute 3 
        public double LearningRate_eta {get;set;} = 0.1;// learning_rate



        private MatrixData _rawData;
        private MatrixData _exemplarData;
        private MatrixData _trainingData;
        private MatrixData _testingData;
        private MatrixData _validationData;

        private MatrixData _confusionMatrixTrain;
        private MatrixData _confusionMatrixTest;  
        private MatrixData _confusionMatrixVal;

        private MatrixData _outputMatrixTrain;
        private MatrixData _outputMatrixTest;  
        private MatrixData _outputMatrixVal;

        private MatrixData _graphData;

        public string ParseCommand(string command)
        {
            throw new NotImplementedException();
        }

        public string LoadData()
        {
            throw new NotImplementedException();
        }

        public string NormalizeData(int col, string method = "StandardScore")
        {
            throw new NotImplementedException();
        }

        public string MakeExemplarData(int col =4, int classes = 3, int startAt = 1)
        {
            throw new NotImplementedException();            
        }

        public string SuffleData(string MatrixData = "_rawData")
        {
            throw new NotImplementedException();            
        }

        public string SplitData(string FromData, string ToData, int NumberOfRows)
        {
            throw new NotImplementedException();
        }

        public string Run()
        {
            string output ="";
            _rawData = new MatrixData("iris - Copy.Txt", false,true,' ');
            _rawData.ChangeHeader(0,"Speal.Length");
            _rawData.ChangeHeader(1,"Speal.Width");
            _rawData.ChangeHeader(2,"Petal.Length");
            _rawData.ChangeHeader(3,"Petal.Width");
            _rawData.ChangeHeader(4,"Species");

            output += "Raw data:"+Environment.NewLine;
            output +=_rawData.Head(5,16);
            output += Environment.NewLine+Environment.NewLine;

            output += "Normalizeing columns 1:4..."+Environment.NewLine;
            _rawData.Normalize(0);
            _rawData.Normalize(1);
            _rawData.Normalize(2);
            _rawData.Normalize(3);
            
            output += "Normalised data:"+Environment.NewLine;
            output +=_rawData.Head(5,16);
            output += Environment.NewLine+Environment.NewLine;

            _exemplarData = _rawData.GetExemplar(4, 3, 1);
            //var _tempExemplarData = new MatrixData()
            output += "Exemplar data:"+Environment.NewLine;
            output +=_exemplarData.Head(5,16);
            output += Environment.NewLine+Environment.NewLine;

            output += "Suffleing data..."+Environment.NewLine;
            _exemplarData.Suffle();
            output += Environment.NewLine;

            output += "Setting trainig data as first 50 rows of suffled exemplar data..."+Environment.NewLine;
            _trainingData = _exemplarData.CopyData(0,0,50);
            output += "Trainig data:"+Environment.NewLine;
            output +=_trainingData.Head(5,16);
            output += Environment.NewLine+Environment.NewLine;

            output += "Setting test data as next 50 rows of suffled exemplar data..."+Environment.NewLine;
            _testingData =_exemplarData.CopyData(50,0,50);
            output +="Test data:"+Environment.NewLine;
            output +=_testingData.Head(5,16);
            output += Environment.NewLine+Environment.NewLine;


            output += "Setting validation data as next 50 rows of suffled exemplar data..."+Environment.NewLine;
            _validationData = _exemplarData.CopyData(100,0,50);
            output += "Validation data:"+Environment.NewLine;
            output +=_validationData.Head(5,16);
            output += Environment.NewLine+Environment.NewLine;


            int num_inputs = NumberInputsNodes; // from iris data set
            int num_hidden = NumberHiddenNodes; // arbitary
            int num_outputs = NumberOutputNodes;// from iris data set
            int epochs = NumberOfEpochs; // For tute 3 
            double eta = LearningRate_eta;// learning_rate

            
            output += "Initialising Neural Network with:"+Environment.NewLine;
            output += num_inputs+" inputs, "+num_hidden+" hidden layers, "+num_outputs+" outputs, "+epochs+" epochs, "+eta+" learning eate"+Environment.NewLine;

            Random rnd1 = new Random(102);
            W4NeuralNetwork nn = new W4NeuralNetwork(num_inputs, num_hidden, num_outputs, rnd1);
            nn.InitializeWeights(rnd1);

            string dir = "Data\\";
            nn.train(_trainingData.Data.ToJagged().ToDoubleArray(), _testingData.Data.ToJagged().ToDoubleArray(), epochs, eta, dir+"nnlog.txt");
            _graphData = nn.GraphData;
        
            double trainAcc = nn.Accuracy(_trainingData.Data.ToJagged().ToDoubleArray(),dir+"trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir+"trainConfusion.txt");
            _confusionMatrixTrain = nn.GetConfusionMatrix();
            _outputMatrixTrain = new MatrixData(dir+"trainOut.txt",false,true,' ');

            double testAcc = nn.Accuracy(_testingData.Data.ToJagged().ToDoubleArray(),dir+"testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            _confusionMatrixTest = nn.GetConfusionMatrix();
            _outputMatrixTest = new MatrixData(dir+"testOut.txt",false,true,' ');

            double valAcc = nn.Accuracy(_validationData.Data.ToJagged().ToDoubleArray(),dir+"valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");
            _confusionMatrixVal = nn.GetConfusionMatrix();
            _outputMatrixVal = new MatrixData(dir+"valOut.txt",false,true,' ');


            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            output += Environment.NewLine;

            output +="Train accuracy = " + trainAcc.ToString("F2")+Environment.NewLine;
            output +="Test accuracy = " + testAcc.ToString("F2")+Environment.NewLine;
            output +="Val accuracy = " + valAcc.ToString("F2")+Environment.NewLine;
            output += Environment.NewLine;
            output +="Train Confusion matrix \r\n"+ConfusionTrain+Environment.NewLine;
            output +="Test Confusion matrix \r\n"+ConfusionTest+Environment.NewLine;
            output +="Val Confusion matrix \r\n"+ConfusionVal+Environment.NewLine;
            GenerateGraph();
            return output;
        }

        public string GenerateGraph()
        {
            double[] x = _graphData.GetColumnCopy<double>(0);
            double[] y = _graphData.GetColumnCopy<double>(1);
            double[] y1 = _graphData.GetColumnCopy<double>(2);
            
            double xMax = _graphData.Max(0);
            double yMax = _graphData.Max(1);
            yMax = (_graphData.Max(2) > yMax)? _graphData.Max(2) : yMax;
            double yMin = _graphData.Min(1);
            yMin = (_graphData.Min(2) < yMin)? _graphData.Min(2): yMin;

            var plot = new PLStream();
            plot.width(1);
            plot.sdev("svg");
            plot.sfnam("Test.svg");
            plot.scolbg(255,255,255);
            plot.init();
            
            plot.env(0,xMax,yMin,yMax+5,AxesScale.Independent,AxisBox.BoxTicksLabelsAxes);

            //####### PLPlot colour guide:
            //0	black (default background)
            //1	red (default foreground)
            //2	yellow
            //3	green
            //4	aquamarine
            //5	pink
            //6	wheat
            //7	grey
            //8	brown
            //9	blue
            //10 BlueViolet
            //11 cyan
            //12 turquoise
            //13 magenta
            //14 salmon
            //15 white


            plot.col0(1);
            plot.lab("Epoch","Accuracy %","Pink = Train, Purple = Test");
            plot.col0(1);

            plot.col0(5);
            plot.line(x,y);
            plot.col0(5);
            
            plot.col0(10);
            plot.line(x,y1);
            plot.col0(10);

            plot.eop();

            return "";
        }

        
    }
}