using nn;
using helpers;
using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Hello World!";
        public string RawData {get {return _ANN.RawData.ToString(20);}}
        public string ExemplarData  {get {return _ANN.ExemplarData.ToString(20);}}
        public string TrainingData  {get {return _ANN.TrainingData.ToString(20);}}
        public string TestingData  {get {return _ANN.TestingData.ToString(20);}}
        public string ValidationData  {get {return _ANN.ValidationData.ToString(20);}}

        public string ConfusionMatrixTrain {get {return _ANN.ConfusionMatrixTrain.ToString(10);}}
        public string ConfusionMatrixTest {get {return _ANN.ConfusionMatrixTest.ToString(10);}}
        public string ConfusionMatrixVal {get {return _ANN.ConfusionMatrixVal.ToString(10);}}

        public string OutputMatrixTrain {get {return _ANN.OutputMatrixTrain.ToString(16);}}
        public string OutputMatrixTest {get {return _ANN.OutputMatrixTest.ToString(16);}}
        public string OutputMatrixVal {get {return _ANN.OutputMatrixVal.ToString(16);}}
        
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

        public void run()
        {
            
        }

        private string _rawOutput = "Application started at: " + DateTime.Now.ToString()+Environment.NewLine;
        private IrisANN _ANN = new IrisANN();

        public MainWindowViewModel()
        {
            Log(_ANN.Run());
        }


        private void Log(string entry)
        {
            RawOutput = RawOutput+entry;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName=null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}
