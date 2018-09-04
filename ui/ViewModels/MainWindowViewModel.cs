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
        public string GraphData {get {return _ANN.GraphData.ToString(20);}}        
        private Dictionary<string, ANN> _n = new Dictionary<string, ANN>();
        public string InputFile {get{return _ANN.InputFile;}set{_ANN.InputFile=value;}}
        public string Delimiter {get {return _ANN.Delimiter.ToString();}set{char val = _ANN.Delimiter; char.TryParse(value.ToCharArray().FirstOrDefault().ToString(), out val); _ANN.Delimiter=val;}}
        public bool HasHeaders {get{return _ANN.HasHeaders;}set{_ANN.HasHeaders=value;}}
        
        public string ColToNormalize{get{return _colToNormalize.ToString();}set{int val=_colToNormalize;int.TryParse(value,out val);_colToNormalize=val;}}
        private int _colToNormalize = 0;
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
            //if the "OnPropertyChanged" method isnt called then the error doesnt occur.
            OnPropertyChanged("CurrentCommand");
        }


        public void ShowPlot()
        {
            string filePath = Directory.GetCurrentDirectory()+"\\Test.svg";
            System.Diagnostics.Process.Start(@"cmd.exe ",@"/c "+filePath);
        }

        public void RunAll()
        {
            Log(_ANN.Run());
            UpdateAllProperties();
        }

        public void ReadFile()
        {
            Log(_ANN.ReadData());
            UpdateAllProperties();
        }

        public void NormalizeData()
        {
            Log(_ANN.NormalizeData(_colToNormalize));
            OnPropertyChanged("RawData");
            
        }

        public void SetExemplar()
        {            
            Log(_ANN.SetExemplar());
            UpdateAllProperties();
        }
        public void SuffleExemplar()
        {            
            Log(_ANN.SuffleExemplar());
            UpdateAllProperties();
        }

        public void SetTrain()
        {            
            Log(_ANN.SetTrain());
            UpdateAllProperties();
        }
        public void SetTest()
        {
            Log(_ANN.SetTest());
            UpdateAllProperties();
        }

        public void SetVal()
        {
            Log(_ANN.SetVal());
            UpdateAllProperties();
        }

        public void RunNetwork()
        {
            Log(_ANN.RunNetwork());
            UpdateAllProperties();
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
