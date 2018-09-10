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
        public void RunCommand(object s = null)
        {
            Log(CurrentCommand);
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
        public void SuffleExemplar()
        {            
            Log(_ANN.SuffleExemplar());
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

        private void SaveState()
        {
            string output = JsonConvert.SerializeObject(_ANN);
            StreamWriter sw = new StreamWriter("state.json");
            sw.Write(output);
            sw.Close();
        }

        private void LoadState()
        {
            //This does not work yet

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Open file"
            };

            var result = ofd.ShowAsync().Result;

            StreamReader sr = new StreamReader(result[0]);
            var file = sr.ReadToEnd();
            sr.Close();
            var x = JsonConvert.DeserializeObject<ANN>(file);
            _ANN = x;
            UpdateAllProperties();
        }

        public void PickInputFile()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select File"
            };

            InputFile = ofd.ShowAsync().Result[0];
            OnPropertyChanged("InputFile");
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
