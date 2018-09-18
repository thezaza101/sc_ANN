using System;
using System.Collections.Generic;
using helpers;
using NeuralNetworks;
using PLplot;
using nn.Parser;

namespace nn
{
    public partial class ANN
    {
        MatrixData noData = new MatrixData(
            new string[][] 
            {
                new string[]{"N","D"},
                new string[]{"o","a"},
                new string[]{" ","t"},
                new string[]{" ","a"}
            }
        );
        public MatrixData RawData {get{return (_rawData==null)? noData : _rawData;}}
        public MatrixData ExemplarData {get{return (_exemplarData==null)? noData : _exemplarData;}}
        public MatrixData TrainingData {get{return (_trainingData==null)? noData : _trainingData;}}
        public MatrixData TestingData {get{return (_testingData==null)? noData : _testingData;}}
        public MatrixData ValidationData {get{return (_validationData==null)? noData : _validationData;}}

        public MatrixData ConfusionMatrixTrain {get{return (_confusionMatrixTrain==null)? noData : _confusionMatrixTrain;}}
        public MatrixData ConfusionMatrixTest {get{return (_confusionMatrixTest==null)? noData : _confusionMatrixTest;}}
        public MatrixData ConfusionMatrixVal {get{return (_confusionMatrixVal==null)? noData : _confusionMatrixVal;}}

        public MatrixData OutputMatrixTrain {get{return (_outputMatrixTrain==null)? noData : _outputMatrixTrain;}}
        public MatrixData OutputMatrixTest {get{return (_outputMatrixTest==null)? noData : _outputMatrixTest;}}
        public MatrixData OutputMatrixVal {get{return (_outputMatrixVal==null)? noData : _outputMatrixVal;}}
        public MatrixData GraphData {get{return (_graphData==null)? noData : _graphData;}}


        public int NumberInputsNodes {get;set;} = 4; // from iris data set
        public int NumberHiddenNodes {get;set;} = 1; // arbitary
        public int NumberOutputNodes {get;set;} = 3;// from iris data set
        public int NumberOfEpochs {get;set;} = 200; // For tute 3 
        public double LearningRate_eta {get;set;} = 0.1;// learning_rate

        public string InputFile {get;set;} = "iris";
        public char Delimiter {get;set;} = ' ';
        public bool HasHeaders {get;set;} = false;

        public int NumTrain {get;set;} = 50;
        public int NumTest {get;set;} = 50;
        public int NumVal {get;set;} = 50;

        private Dictionary<string,object> vars = new Dictionary<string,object>();
        


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

        private ANNParser Parser;

        public ANN()
        {
            Parser = new ANNParser(this);
        }
        public string ParseCommand(string command)
        {
            return Parser.ParseCommand(command);
        }

        
        public string NormalizeData(int col, string method = "StandardScore")
        {
            string output ="";
            output += "Normalizeing column "+col.ToString()+" using " +method+" method."+Environment.NewLine;
            _rawData.Normalize(col);
            return output;
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
        public string ReadData()
        {
            string output ="";
            _rawData = new MatrixData(InputFile, HasHeaders, true,Delimiter);
            output += "Read: "+InputFile+Environment.NewLine;
            output += "Raw data:"+Environment.NewLine;
            output +=_rawData.Head().ToString(5,16,300,true);
            output += Environment.NewLine+Environment.NewLine;
            return output;
        }
        
        public string SetExemplar()
        {
            string output ="";
            _exemplarData = _rawData.GetExemplar(NumberInputsNodes, NumberOutputNodes, 1);
            output += "Exemplar data:"+Environment.NewLine;
            output +=_exemplarData.Head().ToString(5,16,300,true);
            output += Environment.NewLine+Environment.NewLine;
            return output;
        }

        public string SuffleExemplar()
        {
            string output ="";
            output += "Suffleing data..."+Environment.NewLine;
            _exemplarData.Suffle();
            output += Environment.NewLine;
            return output;
        }

        public string SetTrain()
        {
            string output ="";

            _trainingData = _exemplarData.CopyData(0,0,NumTrain);

            output += "Trainig data:"+Environment.NewLine;
            output +=_trainingData.Head().ToString(5,16,300,true);
            output += Environment.NewLine+Environment.NewLine;

            return output;
        }
        public string SetTest()
        {
            string output ="";
            _testingData =_exemplarData.CopyData(NumTrain,0,NumTest);
            output +="Test data:"+Environment.NewLine;
            output +=_testingData.Head().ToString(5,16,300,true);
            output += Environment.NewLine+Environment.NewLine;
            return output;
        }
        public string SetVal()
        {
            string output ="";
            _validationData = _exemplarData.CopyData(NumTrain+NumTest,0,NumVal);
            output += "Validation data:"+Environment.NewLine;
            output +=_validationData.Head().ToString(5,16,300,true);
            output += Environment.NewLine+Environment.NewLine;
            return output;
        }
        public string RunNetwork()
        {
            string output ="";

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
            if(!(_testingData == null))
            {
                nn.train(_trainingData.Data.ToJagged().ToDoubleArray(), _testingData.Data.ToJagged().ToDoubleArray(), epochs, eta, dir+"nnlog.txt");
            }
            else
            {
                nn.train(_trainingData.Data.ToJagged().ToDoubleArray(), _trainingData.Data.ToJagged().ToDoubleArray(), epochs, eta, dir+"nnlog.txt");
            }
            _graphData = nn.GraphData;
        
            double trainAcc = nn.Accuracy(_trainingData.Data.ToJagged().ToDoubleArray(),dir+"trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir+"trainConfusion.txt");
            _confusionMatrixTrain = nn.GetConfusionMatrix();
            _outputMatrixTrain = new MatrixData(dir+"trainOut.txt",false,true,' ');

            double testAcc = 0;
            string ConfusionTest = "";
            bool testDataExists = false;

            if(!(_testingData == null))
            {
                testAcc = nn.Accuracy(_testingData.Data.ToJagged().ToDoubleArray(),dir+"testOut.txt");
                ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
                _confusionMatrixTest = nn.GetConfusionMatrix();
                _outputMatrixTest = new MatrixData(dir+"testOut.txt",false,true,' ');
                testDataExists = true;
            }
            else
            {
                output+= "Testing data is not set, skipping validation step..." + Environment.NewLine;
            }

            double valAcc = 0;
            string ConfusionVal ="";
            bool valDataExists = false;
            
            if(!(_validationData == null))
            {
                valAcc = nn.Accuracy(_validationData.Data.ToJagged().ToDoubleArray(),dir+"valOut.txt");
                ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");
                _confusionMatrixVal = nn.GetConfusionMatrix();
                _outputMatrixVal = new MatrixData(dir+"valOut.txt",false,true,' ');
                valDataExists = true;
            }
            else
            {
                output+= "Validation data is not set, skipping validation step..." + Environment.NewLine;
            }


            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            output += Environment.NewLine;

            output +="Train accuracy = " + trainAcc.ToString("F2")+Environment.NewLine;
            output +=(testDataExists)? "Test accuracy = " + testAcc.ToString("F2")+Environment.NewLine : "";
            output +=(valDataExists)? "Val accuracy = " + valAcc.ToString("F2")+Environment.NewLine : "";
            output += Environment.NewLine;
            output +="Train Confusion matrix \r\n"+ConfusionTrain+Environment.NewLine;
            output += (testDataExists)? "Test Confusion matrix \r\n"+ConfusionTest+Environment.NewLine : "";
            output += (valDataExists)? "Val Confusion matrix \r\n"+ConfusionVal+Environment.NewLine : "";
            GenerateGraph();
            return output;
        }

        public string ResetData()
        {
            string output ="";

            output+="Setting \"" +nameof(_rawData) +"\" to null" + Environment.NewLine;
            _rawData = null;
            output+="Setting \"" +nameof(_exemplarData) +"\" to null" + Environment.NewLine;
            _exemplarData = null;
            output+="Setting \"" +nameof(_trainingData) +"\" to null" + Environment.NewLine;
            _trainingData = null;
            output+="Setting \"" +nameof(_testingData) +"\" to null" + Environment.NewLine;
            _testingData = null;
            output+="Setting \"" +nameof(_validationData) +"\" to null" + Environment.NewLine;
            _validationData = null;

            output+="Setting \"" +nameof(_confusionMatrixTrain) +"\" to null" + Environment.NewLine;
            _confusionMatrixTrain = null;
            output+="Setting \"" +nameof(_confusionMatrixTest) +"\" to null" + Environment.NewLine;
            _confusionMatrixTest = null;  
            output+="Setting \"" +nameof(_confusionMatrixVal) +"\" to null" + Environment.NewLine;
            _confusionMatrixVal = null;
            output+="Setting \"" +nameof(_outputMatrixTrain) +"\" to null" + Environment.NewLine;
            _outputMatrixTrain = null;


            output+="Setting \"" +nameof(_outputMatrixTest) +"\" to null" + Environment.NewLine;
            _outputMatrixTest = null;  
            output+="Setting \"" +nameof(_outputMatrixVal) +"\" to null" + Environment.NewLine;
            _outputMatrixVal = null;

            output+="Setting \"" +nameof(_graphData) +"\" to null" + Environment.NewLine;
            _graphData = null;

            return output;
        }

        public string RunIris()
        {
            string output ="";
            _rawData = new MatrixData("iris", false, true,' ');

            _rawData.ChangeHeader(0,"Speal.Length");
            _rawData.ChangeHeader(1,"Speal.Width");
            _rawData.ChangeHeader(2,"Petal.Length");
            _rawData.ChangeHeader(3,"Petal.Width");
            _rawData.ChangeHeader(4,"Species");

            output += "Raw data:"+Environment.NewLine;
            output +=_rawData.Head().ToString(5,16);
            output += Environment.NewLine+Environment.NewLine;

            output += "Normalizeing columns 1:4..."+Environment.NewLine;
            _rawData.Normalize(0);
            _rawData.Normalize(1);
            _rawData.Normalize(2);
            _rawData.Normalize(3);
            
            output += "Normalised data:"+Environment.NewLine;
            output +=_rawData.Head().ToString(5,16);
            output += Environment.NewLine+Environment.NewLine;

            _exemplarData = _rawData.GetExemplar(4, 3, 1);

            output += "Exemplar data:"+Environment.NewLine;
            output +=_exemplarData.Head().ToString(5,16);
            output += Environment.NewLine+Environment.NewLine;

            output += "Suffleing data..."+Environment.NewLine;
            _exemplarData.Suffle();
            output += Environment.NewLine;

            output += "Setting trainig data as first 50 rows of suffled exemplar data..."+Environment.NewLine;
            _trainingData = _exemplarData.CopyData(0,0,50);
            output += "Trainig data:"+Environment.NewLine;
            output +=_trainingData.Head().ToString(5,16);
            output += Environment.NewLine+Environment.NewLine;

            output += "Setting test data as next 50 rows of suffled exemplar data..."+Environment.NewLine;
            _testingData =_exemplarData.CopyData(50,0,50);
            output +="Test data:"+Environment.NewLine;
            output +=_testingData.Head().ToString(5,16);
            output += Environment.NewLine+Environment.NewLine;


            output += "Setting validation data as next 50 rows of suffled exemplar data..."+Environment.NewLine;
            _validationData = _exemplarData.CopyData(100,0,50);
            output += "Validation data:"+Environment.NewLine;
            output +=_validationData.Head().ToString(5,16);
            output += Environment.NewLine+Environment.NewLine;


            int num_inputs = 4; // from iris data set
            int num_hidden = 1; // arbitary
            int num_outputs = 3;// from iris data set
            int epochs = 200; // For tute 3 
            double eta = 0.1;// learning_rate

            
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
            //double yMax = _graphData.Max(1);
            //yMax = (_graphData.Max(2) > yMax)? _graphData.Max(2) : yMax;
            //double yMin = _graphData.Min(1);
            //yMin = (_graphData.Min(2) < yMin)? _graphData.Min(2): yMin;

            var plot = new PLStream();
            plot.width(1);
            plot.sdev("svg");
            plot.sfnam("Test.svg");
            plot.scolbg(255,255,255);
            plot.init();
            
            plot.env(0,xMax,0,105,AxesScale.Independent,AxisBox.BoxTicksLabelsAxes);

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
            plot.lab("Epoch","Accuracy %","Test vs Train Accuracy");
            plot.ptex(xMax-10,25,1.0,0,1,"Input Nodes: "+NumberInputsNodes);
            plot.ptex(xMax-10,20,1.0,0,1,"Hidden Nodes: "+NumberHiddenNodes);
            plot.ptex(xMax-10,15,1.0,0,1,"Output Nodes: "+NumberOutputNodes);
            plot.ptex(xMax-10,10,1.0,0,1,"Epochs: "+NumberOfEpochs);
            plot.ptex(xMax-10,5,1.0,0,1,"Learning Rate: "+LearningRate_eta);
            plot.col0(1);

            plot.col0(9);
            plot.line(x,y);
            plot.ptex(xMax-10,35,1.0,0,1,"Blue: Train");
            plot.col0(9);
            
            plot.col0(3);
            plot.line(x,y1);
            plot.ptex(xMax-10,30,1.0,0,1,"Green: Test");
            plot.col0(3);

            plot.eop();

            return "Saved \"Test.svg\"";
        }

        

        public string ConvertPlotToString()
        {
            throw new NotImplementedException();
        }

        
    }
}