using System;
using System.Linq;
using System.IO;
using nn;


namespace cli
{
    class Program
    {
        
        private static ANN _ANN = new ANN();
        static void Main(string[] args)
        {
            TaskFivePreProcess t = new TaskFivePreProcess();
            //t.Run("Data\\Process4.csv");
            //t.Run("Data\\FaceDataBoth.csv");

            /*t.ProcessFile("Data\\trainFaceData255.csv", "Data\\1trainFaceDataFinal.csv",1);
            t.ProcessFile("Data\\trainNoFaceData255.csv", "Data\\1trainNoFaceDataFinal.csv",1);
            t.ProcessFile("Data\\testFaceData255.csv", "Data\\1testFaceDataFinal.csv",1);
            t.ProcessFile("Data\\testNoFaceData255.csv", "Data\\1testNoFaceDataFinal.csv",1);

            t.ProcessFile("Data\\trainFaceData255.csv", "Data\\2trainFaceDataFinal.csv",2);
            t.ProcessFile("Data\\trainNoFaceData255.csv", "Data\\2trainNoFaceDataFinal.csv",2);
            t.ProcessFile("Data\\testFaceData255.csv", "Data\\2testFaceDataFinal.csv",2);
            t.ProcessFile("Data\\testNoFaceData255.csv", "Data\\2testNoFaceDataFinal.csv",2);

            t.ProcessFile("Data\\trainFaceData255.csv", "Data\\31trainFaceDataFinal.csv",3);
            t.ProcessFile("Data\\trainNoFaceData255.csv", "Data\\3trainNoFaceDataFinal.csv",3);
            t.ProcessFile("Data\\testFaceData255.csv", "Data\\3testFaceDataFinal.csv",3);
            t.ProcessFile("Data\\testNoFaceData255.csv", "Data\\31testNoFaceDataFinal.csv",3);*/

            //t.ProcessFile("Data\\trainFaceData255.csv", "Data\\4trainFaceDataFinal.csv",4);
            //t.ProcessFile("Data\\trainNoFaceData255.csv", "Data\\4trainNoFaceDataFinal.csv",4);
            //t.ProcessFile("Data\\testFaceData255.csv", "Data\\4testFaceDataFinal.csv",4);
            //t.ProcessFile("Data\\testNoFaceData255.csv", "Data\\4testNoFaceDataFinal.csv",4);

            //t.ProcessFile("Data\\trainFaceData255.csv", "Data\\5trainFaceDataFinal.csv",5);
            //t.ProcessFile("Data\\trainNoFaceData255.csv", "Data\\5trainNoFaceDataFinal.csv",5);
            //t.ProcessFile("Data\\testFaceData255.csv", "Data\\5testFaceDataFinal.csv",5);
            //t.ProcessFile("Data\\testNoFaceData255.csv", "Data\\5testNoFaceDataFinal.csv",5);
            
            //t.GrayTo255("Data\\testNoFaceData.csv");
            //t.GrayTo255("Data\\testFaceData.csv");
            //t.GrayTo255("Data\\trainNoFaceData.csv");
            //t.GrayTo255("Data\\trainFaceData.csv");


            

            if (args.Length > 0)
            {
                ParseScript(args[0]);
            }
            while(Console.ReadLine()!="quit")
            {
                ParseCommand(Console.ReadLine());
            }
        }
        static void ParseScript(string scriptPath)
        {
            using (var sr = new StreamReader(scriptPath.Trim()))
                {
                    while (!sr.EndOfStream)
                    {
                        ParseCommand(sr.ReadLine().Trim());
                    }
                }
        }   
        static void ParseCommand(string command)
        {
            System.Console.WriteLine(_ANN.ParseCommand(command));
        }     
    }
}
