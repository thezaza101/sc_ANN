using System;
using System.IO;
namespace nn
{ 
    public partial class NeuralNetwork
    {
        /*
        public void saveANN(string fileName)
        {
            //double[] weights = GetWeights();

            StreamWriter writer = null;
            writer = new StreamWriter(fileName);

            // save setup info 
            writer.WriteLine(numInput.ToString() + " " + numHidden.ToString() + " " + numOutput.ToString());

            //Save hidden layer
            for (int i = 0; i < numInput; i++) appendVectorToFile(writer, ihWeights[i]);
            appendVectorToFile(writer, hBiases);

            //save output layer
            for (int i = 0; i < numHidden; i++) appendVectorToFile(writer, hoWeights[i]);
            appendVectorToFile(writer, oBiases);

            writer.Close();
        } */
        public static double[] stringToVector(string str, int itemCount, char[] delimiters)
        {
            double[] retv = new double[itemCount];
            str = str.Trim();
            if (str == "") return retv;
            string[] ss = str.Split(delimiters);
            int cnt = 0;
            double d=0;
            foreach (string s in ss)
            {
                try
                {
                    d = Double.Parse(s);
                }
                catch (Exception e)
                {
                    errors += "The line '" + str + "' contains an invalid number " + e.ToString() + Environment.NewLine;
                    return retv;
                }
                retv[cnt] = d;
                cnt++;
            }
            return retv;
        }

        /*
        public void loadANN(string fileName)
        {
            //double[] weights = GetWeights();

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fileName);

                // save setup info 
                string setupInfo = reader.ReadLine();
                //writer.WriteLine(numInput.ToString() + " " + numHidden.ToString() + " " + numOutput.ToString());
                double[] d = stringToVector(setupInfo, 3, delimit);

                int numInputCheck = (int)d[0];
                int numHiddenCheck = (int)d[1];
                int numOutputCheck = (int)d[2];

                if (numInputCheck != numInput || 
                    numHiddenCheck != numHidden ||
                    numOutputCheck != numOutput)
                {
                    errors += "This network has different number of nodes to the one on the file - loadANN aborted !";
                    reader.Close();
                    return;
                }
                string line = "";
                //double[] ihWeights

                for (int i = 0; i < numInput; i++)
                {
                    line = reader.ReadLine();
                    ihWeights[i] = stringToVector(line, numHidden, delimit);
                    //appendVectorToFile(writer, ihWeights[i]);
                }
                line = reader.ReadLine();
                hBiases = stringToVector(line, numHidden, delimit);
                //appendVectorToFile(writer, hBiases);

                //save output layer
                for (int i = 0; i < numHidden; i++)
                {
                    line = reader.ReadLine();
                    hoWeights[i] = stringToVector(line, numOutput, delimit);
                    //appendVectorToFile(writer, hoWeights[i]);
                }
                line = reader.ReadLine();
                oBiases = stringToVector(line, numOutput, delimit);
                //appendVectorToFile(writer, oBiases);

                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: {0}", e.Message), e);
            }
        }
         */
    }
}