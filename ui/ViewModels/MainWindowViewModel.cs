using nn;
using helpers;
using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;

namespace ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public string Greeting {get{return "resm:ui.Test.png";}} 
        public string RawData {get {return _ANN.RawData.ToString(20);}}
        public string ExemplarData  {get {return _ANN.ExemplarData.ToString(20);}}
        public string TrainingData  {get {return _ANN.TrainingData.ToString(20);}}
        public string TestingData  {get {return _ANN.TestingData.ToString(20);}}
        public string ValidationData  {get {return _ANN.ValidationData.ToString(20);}}

        public string NumberInputsNodes {get { return _ANN.NumberInputsNodes.ToString();}set{int val=_ANN.NumberInputsNodes;int.TryParse(value,out val);_ANN.NumberInputsNodes=val;}}
        public string NumberHiddenNodes {get { return _ANN.NumberHiddenNodes.ToString();}set{int val=_ANN.NumberHiddenNodes;int.TryParse(value,out val);_ANN.NumberHiddenNodes=val;}}
        public string NumberOutputNodes {get { return _ANN.NumberOutputNodes.ToString();}set{int val=_ANN.NumberOutputNodes; int.TryParse(value,out val);_ANN.NumberOutputNodes=val;}}
        public string NumberOfEpochs {get { return _ANN.NumberOfEpochs.ToString();}set{int val=_ANN.NumberOfEpochs; int.TryParse(value,out val); _ANN.NumberOfEpochs=val;}}
        public string LearningRate_eta {get { return _ANN.LearningRate_eta.ToString();}set{double val=_ANN.LearningRate_eta; double.TryParse(value, out val);_ANN.LearningRate_eta=val;}}

        public string ConfusionMatrixTrain {get {return _ANN.ConfusionMatrixTrain.ToString(10);}}
        public string ConfusionMatrixTest {get {return _ANN.ConfusionMatrixTest.ToString(10);}}
        public string ConfusionMatrixVal {get {return _ANN.ConfusionMatrixVal.ToString(10);}}

        public string OutputMatrixTrain {get {return _ANN.OutputMatrixTrain.ToString(16);}}
        public string OutputMatrixTest {get {return _ANN.OutputMatrixTest.ToString(16);}}
        public string OutputMatrixVal {get {return _ANN.OutputMatrixVal.ToString(16);}}
        public string GraphData {get {return _ANN.GraphData.ToString(_ANN.GraphData.NumberOfRows, 20, 300);}}        
        public string InputFile {get{return _ANN.InputFile;}set{_ANN.InputFile=value;}}
        public string Delimiter {get {return _ANN.Delimiter.ToString();}set{char val = _ANN.Delimiter; char.TryParse(value.ToCharArray().FirstOrDefault().ToString(), out val); _ANN.Delimiter=val;}}
        public bool HasHeaders {get{return _ANN.HasHeaders;}set{_ANN.HasHeaders=value;}}
        
        public string ColToNormalizeFrom{get{return _colToNormalizeFrom.ToString();}set{int val=_colToNormalizeFrom;int.TryParse(value,out val);_colToNormalizeFrom=val;}}
        private int _colToNormalizeFrom = 0;

        public bool UseRandomSeed {get{return _useRandomSeed;}set{_useRandomSeed=value;}}
        private bool _useRandomSeed = false;



        public string RandomSeed{get{return _randomSeed.ToString();}set{int val=_randomSeed;int.TryParse(value,out val);_randomSeed=val;}}
        private int _randomSeed = 0;

        public string ColToNormalizeTo{get{return _colToNormalizeTo.ToString();}set{int val=_colToNormalizeTo;int.TryParse(value,out val);_colToNormalizeTo=val;}}
        private int _colToNormalizeTo = 3;
        public string NumTrain {get { return _ANN.NumTrain.ToString();}set{int val=_ANN.NumTrain;int.TryParse(value,out val);_ANN.NumTrain=val;}}
        public string NumTest {get { return _ANN.NumTest.ToString();}set{int val=_ANN.NumTest;int.TryParse(value,out val);_ANN.NumTest=val;}}
        public string NumVal {get { return _ANN.NumVal.ToString();}set{int val=_ANN.NumVal; int.TryParse(value,out val);_ANN.NumVal=val;}}

        public string RawOutput
        {
            get
            {
                return _rawOutput;
            }
            set 
            {
                if(value != _rawOutput)
                {
                    _rawOutput = value+Environment.NewLine;
                    OnPropertyChanged();
                }
            }            
        }

        public string CurrentCommand 
        {
            get
            {
                return _currentCommand;
            }
            set 
            {
                if(value != _currentCommand)
                {
                    _currentCommand = value;
                    OnPropertyChanged();
                }
            }  

        }
        private string _currentCommand = "";

        private string _rawOutput = "Application started at: " + DateTime.Now.ToString()+Environment.NewLine;
        private ANN _ANN = new ANN();
        public MainWindowViewModel()
        {
            
        }
        public void RunCommand(string commandOverride = "")
        {
            string cmd = (string.IsNullOrWhiteSpace(commandOverride))? _currentCommand : commandOverride ;
            if (!(cmd==commandOverride)) {Log("> "+cmd);}

            //Delete me later
            if(_currentCommand=="xx") SaveExemplar();
            if(_currentCommand=="yy") SetDataAsExemplar();


            Log(_ANN.ParseCommand(cmd));
            _currentCommand = "";


            OnPropertyChanged("CurrentCommand");
        }
        
        public void ClearRawData()
        {
            _rawOutput = "";
            OnPropertyChanged("RawOutput");

        }

        public void ResetANNMatrixData()
        {
            Log(_ANN.ResetData());
            UpdateAllProperties();

        }

        public void GeneratePlot()
        {
            Log(_ANN.GenerateGraph());
            OnPropertyChanged("RawOutput");
        }

        public void ShowPlot()
        {
            string filePath = Directory.GetCurrentDirectory()+"\\Test.svg";
            System.Diagnostics.Process.Start(@"cmd.exe ",@"/c "+filePath);
        }

        public void RunAll()
        {
            ReadFile();
            NormalizeData();
            SetExemplar();
            SuffleExemplar();
            SetTrain();
            SetTest();
            SetVal();
            RunNetwork();
            GeneratePlot();
        }
        public void RunAllNewData()
        {
            SuffleExemplar();
            SetTrain();
            SetTest();
            SetVal();
            RunNetwork();
        }

        public void RunIris()
        {
            Log(_ANN.RunIris());
            UpdateAllProperties();
        }

        public void ReadFile()
        {
            Log(_ANN.ReadData());
            OnPropertyChanged("RawOutput");
            OnPropertyChanged("RawData");
        }

        public void NormalizeData()
        {
            for (int i = _colToNormalizeFrom; i<=_colToNormalizeTo;i++)
            {
                Log(_ANN.NormalizeData(i));
            }
            OnPropertyChanged("RawOutput");
            OnPropertyChanged("RawData");
        }

        public void SetExemplar()
        {            
            Log(_ANN.SetExemplar());
            UpdateAllProperties();
            OnPropertyChanged("RawOutput");
            OnPropertyChanged("ExemplarData");
            
        }
        public void SaveExemplar()
        {
            _ANN.ExemplarData.WriteCSV("ex.csv");

        }

        public void SetDataAsExemplar()
        {
            _ANN.SetExemplarFromMatrix(new MatrixData(_ANN.RawData,0,0));
            OnPropertyChanged("ExemplarData");
        }
        public void SuffleExemplar()
        {   
            int? val = _useRandomSeed? _randomSeed : (int?)null;
            Log(_ANN.SuffleExemplar(val));
            UpdateAllProperties();
            OnPropertyChanged("RawOutput");
            OnPropertyChanged("ExemplarData");
        }

        public void SetTrain()
        {            
            Log(_ANN.SetTrain());
            UpdateAllProperties();
             OnPropertyChanged("RawOutput");
            OnPropertyChanged("TrainingData");
        }
        public void SetTest()
        {
            Log(_ANN.SetTest());
            UpdateAllProperties();
             OnPropertyChanged("RawOutput");
            OnPropertyChanged("TestingData");
        }

        public void SetVal()
        {
            Log(_ANN.SetVal());
            UpdateAllProperties();
             OnPropertyChanged("RawOutput");
            OnPropertyChanged("ValidationData");
        }

        public void RunNetwork()
        {
            Log(_ANN.RunNetwork());
            OnPropertyChanged("RawOutput");
            OnPropertyChanged("ConfusionMatrixTrain");
            OnPropertyChanged("ConfusionMatrixTest");
            OnPropertyChanged("ConfusionMatrixVal");
            OnPropertyChanged("OutputMatrixTrain");
            OnPropertyChanged("OutputMatrixTest");
            OnPropertyChanged("OutputMatrixVal");
            OnPropertyChanged("GraphData");
        }

        public void PickInputFile()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select File"
            };
            try
            {
                InputFile = ofd.ShowAsync().Result[0];                
            }
            catch
            {

            }

            OnPropertyChanged("InputFile");
        }

        public void LoadScript()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select File"
            };
            ofd.Filters.Add(new FileDialogFilter() { Name = "CSCS Script file", Extensions = { "cscs" } });
            string scriptFile = null;
            try
            {
                scriptFile = ofd.ShowAsync().Result[0];
            } catch {}

            if(!string.IsNullOrEmpty(scriptFile))
            {
                using (var sr = new StreamReader(scriptFile))
                {
                    List<string> file = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        file.Add(sr.ReadLine());
                    }

                    foreach(string s in file)
                    {
                        RunCommand(s.Trim());
                        OnPropertyChanged("RawOutput");
                    }
                }
            }
            
        }

        private void Log(string entry)
        {
            RawOutput = RawOutput+entry;

        }

        private void UpdateAllProperties()
        {
            foreach (PropertyInfo p in  typeof(MainWindowViewModel).GetProperties())
            {
                OnPropertyChanged(p.Name);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName=null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}
