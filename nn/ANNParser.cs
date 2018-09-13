using System;
using System.Text.RegularExpressions;
using System.Text;
using helpers;
using System.Collections.Generic;
using System.Linq;
using nn;
using System.IO;
using NeuralNetworks;
using PLplot;
namespace nn.Parser
{
    public class ANNParser
    {
        /*
        set _rawData = LoadFileAsMatrix("Data\Wine.txt"|F|',');
        set _inputs = int(1);
        set _copyVar = var(_rawData);
        @ Print($_copyVar);

        @ Print(1);

        @ Print($_rawData);
         */
        private ANN _ANN;
        public ANNParser(ANN baseANN)
        {
            _ANN = baseANN;
        }
        public string ParseCommand(string command)
        {
            if(command.Contains("generateNNGraph"))
            {

            }
            //set, has lhs(varName) and rhs(Command)            
            string[] commandComp = SafeSplitSpaces(command);

            if (string.IsNullOrWhiteSpace(command)) return " ";
            switch (commandComp[0])
            {
                case "set":
                    return SetCommand(commandComp);
                case "@":
                    return CallCommand(commandComp);
                case "//":
                    return command;
                default:
                    return "Error parsing command: \"" + commandComp[0] + "\"";
            }
        }

        private string SetCommand(string[] commandComp)
        {
            //set _rawData = LoadFileAsMatrix("",T,' ');
            string[] rhs = (commandComp.Count(c=>c=="(")==1)? commandComp[3].Split('(') : commandComp[3].Split(new[] { '(' },2);
            string varname = commandComp[1];
            string commandName = rhs[0];
            string[] args = ParseArgs(rhs[1]);
            string outputRHS = "";
            switch (commandName)
            {
                case "loadFileAsMatrix":
                    MatrixData dataToAdd = loadFileAsMatrix(args[0], sb(args[1]), sc(args[2]));
                    outputRHS += (_ANN.SetVar(varname, dataToAdd)) ? "set as new" : "replaced with";
                    outputRHS += " " + dataToAdd.NumberOfRows + "x" + dataToAdd.NumberOfColumns + " MatrixData";
                    break;
                case "copyMatrix":
                    MatrixData copiedData = copyMatrix(args[0],si(args[1]), si(args[2]), si(args[3]), si(args[4]));
                    outputRHS += (_ANN.SetVar(varname, copiedData)) ? "set as new" : "replaced with";
                    outputRHS += " " + copiedData.NumberOfRows + "x" + copiedData.NumberOfColumns + " MatrixData from "+args[0];
                    break;
                case "getExemplar":
                    MatrixData exepData = getExemplar(args[0], si(args[1]),si(args[2]), si(args[3]),args[4]);
                    outputRHS += (_ANN.SetVar(varname, exepData)) ? "set as new" : "replaced with";
                    outputRHS += " " + exepData.NumberOfRows + "x" + exepData.NumberOfColumns + " MatrixData from "+args[0];
                    break;
                case "var":
                    outputRHS += (_ANN.SetVar(varname, _ANN.GetVar(args[0].Replace("$", "")))) ? "set as new" : "replaced with";
                    outputRHS += " $" + args[0].Replace("$", "");
                    break;
                case "int":
                    outputRHS += (_ANN.SetVar(varname, si(args[0]))) ? "set to" : "replaced with";
                    outputRHS += " " + _ANN.GetVarAs<int>(varname).ToString();
                    break;
                case "double":
                    outputRHS += (_ANN.SetVar(varname, sd(args[0]))) ? "set to" : "replaced with";
                    outputRHS += " " + _ANN.GetVarAs<double>(varname).ToString();
                    break;
                case "string":
                    outputRHS += (_ANN.SetVar(varname, (string)Value(args[0]))) ? "set to" : "replaced with";
                    outputRHS += " " + _ANN.GetVarAs<string>(varname).ToString();
                    break;
                case "radomGen":
                    Random rand = radomGen(si(args[0]));
                    outputRHS += (_ANN.SetVar(varname, rand)) ? "set to" : "replaced with";
                    outputRHS += " an instance of a \"Random\" object with seed "+args[0];
                    break;
                case "nuralNetwork":
                    var nn = nuralNetwork(si(args[0]),si(args[1]),si(args[2]), (Random)_ANN.GetVar((args[3])));
                    outputRHS += (_ANN.SetVar(varname, nn)) ? "set to" : "replaced with";
                    outputRHS += " an instance of a \"Nural Network\" object with "+si(args[0])+" inputs, "+si(args[1])+" hidden, "+si(args[2])+" outputs.";
                    break;
                case "getGraphData":
                    var gd = getGraphData(args[0]);
                    outputRHS += (_ANN.SetVar(varname, gd)) ? "set to" : "replaced with";
                    outputRHS += " graph data from "+args[0];
                    break;
                case "getConfusionMatrix":
                    var cm = getConfusionMatrix(args[0],(string)Value(args[1]),(string)Value(args[2]));
                    outputRHS += (_ANN.SetVar(varname, cm)) ? "set to" : "replaced with";
                    outputRHS += " confusion matrix from "+args[0];
                    break;   

                

                default:
                    return "the LHS 'set' command cannot be used with RHS command: \"" + commandName + "\"";
            }
            return "Variable \"" + varname + "\" " + outputRHS;
        }
        private string CallCommand(string[] commandComp)
        {
            string[] rhs = (commandComp.Count(c=>c=="(")>1)? commandComp[1].Split(new[] { '(' },2) : commandComp[1].Split('(');

            string commandName = rhs[0];
            string[] args = ParseArgs(rhs[1]);
            switch (commandName)
            {
                case "print": return print(args[0]);
                case "updateMatrixHeader": return updateMatrixHeader(args[0], si(args[1]), args[2]);
                case "normalizeMatrixCol": return normalizeMatrixCol(args[0], si(args[1]), args[2]);
                case "normalizeMatrixCols": return normalizeMatrixCols(args[0], si(args[1]),si(args[2]), args[3]);         
                case "suffleMatrix": return suffleMatrix(args[0]);      
                case "add": return add(args[0],args[1]).ToString();
                case "subtract": return subtract(args[0],args[1]).ToString();
                case "multiply": return multiply(args[0],args[1]).ToString();
                case "devide": return devide(args[0],args[1]).ToString();
                case "initializeNNWeights": return initializeNNWeights(args[0],(Random)_ANN.GetVar((args[1])));
                case "trainNN": return trainNN(args[0],args[1],args[2],si(args[3]),sd(args[4]),(string)Value(args[5]));
                case "getNNAccuracy": return getNNAccuracy(args[0],args[1],(string)Value(args[2]),(string)Value(args[3])).ToString();
                case "generateNNGraph": return generateNNGraph(args[0],si(args[1]),si(args[2]),si(args[3]),si(args[4]),sd(args[5]),(string)Value(args[6]),
                si(args[7]),(string)Value(args[8]),si(args[9]),(string)Value(args[10]),(string)Value(args[11]));
                case "runProcess": return runProcess((string)Value(args[0]));
                default: return "the LHS 'call' command cannot be used with RHS command: \"" + commandName + "\"";
            }
        }

        public string runProcess(string processDir)
        {
            System.Diagnostics.Process.Start(@"cmd.exe ",@"/c "+processDir);
            return "Starting process..." + processDir;
        }

        public string print(string input)
        {
            return Value(input).ToString();
        }

        private object Value(string input)
        {
            if (input.StartsWith("@"))
            {
                return ParseCommand(input.Insert(1," "));
            }
            else
            {
                if (input.Contains('$'))
                {
                    try
                    {
                        string[] inputSplit = input.Replace("$", "").Split('.');
                        var foundObj = _ANN.GetVar(inputSplit[0]);
                        if (inputSplit.Length <= 1)
                        {
                            return foundObj;
                        }
                        else
                        {
                            var x = foundObj.GetType().GetProperty(inputSplit[1]);
                            var y = x.GetValue(foundObj, null);
                            return y;
                        }
                    }
                    catch (KeyNotFoundException e)
                    {
                        return e.Message;
                    }
                }
                else
                {
                    return input;
                }
            }
        }

        private string[] ParseArgs(string args)
        {
            if (args.Contains("@"))
            {
                string strRegex = @"\|\s*(?![^()]*\))";
                Regex myRegex = new Regex(strRegex, RegexOptions.Multiline);
                string[] output =  myRegex.Split(args);
                return output;
            }
            else
            {
                return args.Replace("(", "")
                .Replace(")", "")
                .Replace(";", "")
                .Replace("\"", "")
                .Replace("'", "")
                .Split('|');
            }
            
        }

        //string to bool
        private bool sb(string input)
        {
            switch (((string)Value(input)).ToLower())
            {
                case "t":
                    return true;
                case "true":
                    return true;
                case "1":
                    return true;
                default:
                    return false;

            }
        }

        //string to char
        private char sc(string input)
        {
            return ((string)Value(input)).ToCharArray()[0];
        }

        //String to int
        private int si(string input)
        {
            int output = 0;
            int.TryParse(Value(input).ToString(), out output);
            return output;
        }

        //string to double
        private double sd(string input)
        {
            double output = 0;
            double.TryParse(Value(input).ToString(), out output);
            return output;
        }

        private double add(string input1, string input2)
        {
            double v1 = 0;
            double v2 = 0;
            double.TryParse(Value(input1).ToString(), out v1);
            double.TryParse(Value(input2).ToString(), out v2);            
            return v1+v2;
        }
        private double subtract(string input1, string input2)
        {
            double v1 = 0;
            double v2 = 0;
            double.TryParse(Value(input1).ToString(), out v1);
            double.TryParse(Value(input2).ToString(), out v2);            
            return v1-v2;
        }
        private double multiply(string input1, string input2)
        {
            double v1 = 0;
            double v2 = 0;
            double.TryParse(Value(input1).ToString(), out v1);
            double.TryParse(Value(input2).ToString(), out v2);            
            return v1*v2;
        }
        private double devide(string input1, string input2)
        {
            double v1 = 0;
            double v2 = 0;
            double.TryParse(Value(input1).ToString(), out v1);
            double.TryParse(Value(input2).ToString(), out v2);            
            return v1*v2;
        }



        private MatrixData loadFileAsMatrix(string fileName, bool headers, char dmim)
        {
            return new MatrixData(fileName, headers, true, dmim);
        }

        private MatrixData copyMatrix(string matrixRef, int rowStart, int colStart, int numRow, int numCols)
        {
            var md = Value(matrixRef);
            if (md.GetType() == typeof(string))
            {
                return new MatrixData(new string[]{"E","r","r","o","r"});
            }
            MatrixData m = (MatrixData)md;
            return m.CopyData(rowStart,colStart,numRow,numCols);
        }
        private MatrixData getExemplar(string matrixRef, int inputs,int outputs, int startAt, string colName)
        {
            var md = Value(matrixRef);
            if (md.GetType() == typeof(string))
            {
                return new MatrixData(new string[]{"E","r","r","o","r"});
            }
            MatrixData m = (MatrixData)md;

            return m.GetExemplar(inputs, outputs,startAt,colName);
        }
        private MatrixData getGraphData(string nnRef)
        {
            var nn = Value(nnRef);
            if (nn.GetType() == typeof(string))
            {
                return new MatrixData(new string[]{"E","r","r","o","r"});
            }

            W4NeuralNetwork m = (W4NeuralNetwork)nn;
            return m.GraphData;
        }

        private Random radomGen(int seed)
        {
            return new Random(seed);   
        }
        private W4NeuralNetwork nuralNetwork (int inputs, int hidden, int outputs, Random r)
        {
            return new W4NeuralNetwork(inputs, hidden, outputs, r);
        }
        private string updateMatrixHeader(string matrixRef, int colToUpdate, string headerName)
        {
            var md = Value(matrixRef);
            if (md.GetType() == typeof(string))
            {
                return md.ToString();
            }
            MatrixData m = (MatrixData)md;
            m.ChangeHeader(colToUpdate, headerName);
            return "Updated column " + colToUpdate + " of " + matrixRef + " to " + headerName;
        }
        private string normalizeMatrixCol(string matrixRef, int colToUpdate, string methodString)
        {
            var md = Value(matrixRef);
            if (md.GetType() == typeof(string))
            {
                return md.ToString();
            }
            MatrixData m = (MatrixData)md;

            Enum.TryParse(methodString, out NormalizationMethod method);


            m.Normalize(colToUpdate, method);
            return "Normalize column " + colToUpdate + " of " + matrixRef + " using the " + methodString  +" method";
        }
        private string normalizeMatrixCols(string matrixRef, int colToUpdateFrom,int colToUpdateto, string methodString)
        {
            var md = Value(matrixRef);
            if (md.GetType() == typeof(string))
            {
                return md.ToString();
            }
            MatrixData m = (MatrixData)md;

            Enum.TryParse(methodString, out NormalizationMethod method);
            for (int i = colToUpdateFrom; i<=colToUpdateto; i++)
            {
                m.Normalize(i, method);
            }
            return "Normalized columns " + colToUpdateFrom +"-" +colToUpdateto+ " of " + matrixRef + " using the " + methodString  +" method";
        }
        private string suffleMatrix(string matrixRef)
        {
            var md = Value(matrixRef);
            if (md.GetType() == typeof(string))
            {
                return md.ToString();
            }
            MatrixData m = (MatrixData)md;
            m.Suffle();
            return "Suffled matrix \"" + matrixRef + "\"";
        }
        private string initializeNNWeights(string nnRef, Random r)
        {
            var nn = Value(nnRef);
            if (nn.GetType() == typeof(string))
            {
                return nn.ToString();
            }
            W4NeuralNetwork m = (W4NeuralNetwork)nn;
            m.InitializeWeights(r);
            return "Initialize weights of \"" + nnRef + "\"";
        }
        private string trainNN(string nnRef, string matrixTrainRef, string matrixTestRef, int epochs, double eta, string outputFileName)
        {
            var nn = Value(nnRef);
            if (nn.GetType() == typeof(string))
            {
                return nn.ToString();
            }
            var matrixTrain = Value(matrixTrainRef);
            if (matrixTrain.GetType() == typeof(string))
            {
                return matrixTrain.ToString();
            }
            var matrixTest = Value(matrixTrainRef);
            if (matrixTest.GetType() == typeof(string))
            {
                return matrixTest.ToString();
            }

            MatrixData train = (MatrixData)matrixTrain;
            MatrixData test = (MatrixData)matrixTest;
            W4NeuralNetwork m = (W4NeuralNetwork)nn;

            m.train(train.Data.ToJagged().ToDoubleArray(), test.Data.ToJagged().ToDoubleArray(), epochs, eta, outputFileName+".nnlog.txt");
            return "Trained neural network \"" + nnRef + "\" with "+epochs+" epochs, " + eta.ToString() +" eta";
        }
        
        private double getNNAccuracy(string nnRef, string matrixRef, string outputDir, string fileName)
        {
            var nn = Value(nnRef);
            if (nn.GetType() == typeof(string))
            {
                return 0.0;
            }
            var matrixComp = Value(matrixRef);
            if (matrixComp.GetType() == typeof(string))
            {
                return 0.0;
            }
            MatrixData comp = (MatrixData)matrixComp;
            W4NeuralNetwork m = (W4NeuralNetwork)nn;
            return m.Accuracy(comp.Data.ToJagged().ToDoubleArray(),outputDir+fileName+".txt");
        }
        
        private MatrixData getConfusionMatrix(string nnRef, string outputDir, string outputFileName)
        {
            var nn = Value(nnRef);
            if (nn.GetType() == typeof(string))
            {
                return new MatrixData(new string[]{"E","r","r","o","r"});
            }

            W4NeuralNetwork m = (W4NeuralNetwork)nn;
            m.showConfusion(outputDir+outputFileName+"confusion.txt");
            return new MatrixData(m.GetConfusionMatrix(),0,0,0,0);
        }
        private string[] SafeSplitSpaces(string input)
        {
            string strRegex = @"[ ](?=(?:[^""]*""[^""]*"")*[^""]*$)";
            Regex myRegex = new Regex(strRegex, RegexOptions.Multiline);
            return myRegex.Split(input);
        }

        
        public string generateNNGraph(string matrixRef, int NumberInputsNodes, int NumberHiddenNodes, int NumberOutputNodes,
        int NumberOfEpochs, double LearningRate_eta, string title, int oneCol, string oneLab, int twoCol, string twoLab, string fileName)
        {
            var matrixComp = Value(matrixRef);
            if (matrixComp.GetType() == typeof(string))
            {
                return "Could not find variable "+matrixRef;
            }
            MatrixData _graphData = (MatrixData)matrixComp;
            double[] x = _graphData.GetColumnCopy<double>(0);
            double[] y = _graphData.GetColumnCopy<double>(1);
            double[] y1 = _graphData.GetColumnCopy<double>(2);
            
            double xMax = _graphData.Max(0);

            var plot = new PLStream();
            plot.width(1);
            plot.sdev("svg");
            plot.sfnam(fileName+".svg");
            plot.scolbg(255,255,255);
            plot.init();
            
            plot.env(0,xMax,0,105,AxesScale.Independent,AxisBox.BoxTicksLabelsAxes);

            Dictionary<int,string> cols = new Dictionary<int, string>();
            cols.Add(0,"black");
            cols.Add(1,"red");
            cols.Add(2,"yellow");
            cols.Add(3,"green");
            cols.Add(4,"aquamarine");
            cols.Add(5,"pink");
            cols.Add(6,"wheat");
            cols.Add(7,"grey");
            cols.Add(8,"brown");
            cols.Add(9,"blue");
            cols.Add(10,"BlueViolet");
            cols.Add(11,"cyan");
            cols.Add(12,"turquoise");
            cols.Add(13,"magenta");
            cols.Add(14,"salmon");
            cols.Add(15,"white");


            plot.col0(1);
            plot.lab("Epoch","Accuracy %",title);
            plot.ptex(xMax-10,25,1.0,0,1,"Input Nodes: "+NumberInputsNodes);
            plot.ptex(xMax-10,20,1.0,0,1,"Hidden Nodes: "+NumberHiddenNodes);
            plot.ptex(xMax-10,15,1.0,0,1,"Output Nodes: "+NumberOutputNodes);
            plot.ptex(xMax-10,10,1.0,0,1,"Epochs: "+NumberOfEpochs);
            plot.ptex(xMax-10,5,1.0,0,1,"Learning Rate: "+LearningRate_eta);
            plot.col0(1);

            plot.col0(oneCol);
            plot.line(x,y);
            plot.ptex(xMax-10,35,1.0,0,1,cols[oneCol]+ ": "+oneLab);
            plot.col0(oneCol);
            
            plot.col0(twoCol);
            plot.line(x,y1);
            plot.ptex(xMax-10,30,1.0,0,1,cols[twoCol]+": "+twoLab);
            plot.col0(twoCol);

            plot.eop();

            return Directory.GetCurrentDirectory()+"\\"+fileName+".svg";
        }
    }
}