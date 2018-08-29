using helpers;
namespace nn
{
    public interface I_NeuralNetwork
    {             
        string GetProperty(string Key);
        bool SetProperty(string Key, string Value);
        MatrixData GetTrainingData();
        double[][] GetTestingData();
    }
}
