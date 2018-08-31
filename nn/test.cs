using System;

namespace nn
{
    public class OANN
    {
        public double LearningRate {get;set;} = 0.5;
        public int NumberOfInputs {get; private set;}
        public int NumberOfHidden {get; private set;}

        public int NumberOfOutputs {get; private set;}
        //public int NumberOfInputs {get; private set;}


        public OANN(int num_inputs, int num_hidden, int num_outputs, 
        object hidden_layer_weights = null, object hidden_layer_bias = null,
        object output_layer_weights = null, object output_layer_bias = null)
        {

        }
    }
}