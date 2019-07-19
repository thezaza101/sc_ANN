using System;
using System.Linq;
using System.IO;
using nn;
using helpers;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;


namespace cli
{
    class Program
    {
        
        private static ANN _ANN = new ANN();
        private static MatrixData vm = CreateVignetteMask((int)(1920/8), (int)(1080/8),255);
        private static MatrixData CreateVignetteMask(int Width, int Height, double strength)
        {
            MatrixData output = new MatrixData(Width, Height);
            double xLoc;
            double yLoc;
            double dist;
            for(int r = 0; r < output.NumberOfRows; r++)
            {
                for (int c = 0; c<output.NumberOfColumns; c++)
                {
                    xLoc = (double)c / Width;
                    yLoc = (double)r / Height;     
                    dist = Math.Sqrt(Math.Abs(Math.Pow((Width / 2) - r, 2) + Math.Pow((Height / 2)-c, 2))) / ((Width+Height)/2);
                    output[r,c] = (1- (dist * strength))*-1;

                }
            }
            return output;
        }
        static void Main(string[] args)
        {
            
            for (int i = 0; i <=11; i++)
            {
                PI(i);
            }
            
            

            //TaskFivePreProcess t = new TaskFivePreProcess();
            //t.Run("Data\\testFaceData255.csv");

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
        static void PI (int img)
        {
            System.Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Image<Rgba32> image = Image.Load("data\\"+img+".bmp");
            image.Mutate(ctx=>ctx.Resize(image.Width / 8, image.Height / 8));

            double ConversionFactor = 255 / (5 - 1);
            double AverageValue;
            MatrixData m = new MatrixData (image.Width,image.Height);
           
            for(int r = 0; r < image.Width; r++)
            {
                for (int c = 0; c<image.Height; c++)
                {
                    AverageValue = (image[r,c].R + image[r,c].B + image[r,c].G) / 3;
                    double pv = ((AverageValue / ConversionFactor) + 0.5) * ConversionFactor;
                    if (pv < 200) pv = 0;
                    m[r,c] = pv;
                }
            }
            
            m = m.HistEQ();
            int mp = (int)m.MeanIfPixel((s) => {return (s>0);});
            
            for(int r = 0; r < image.Width; r++)
            {
                for (int c = 0; c<image.Height; c++)
                {
                    if (m[r,c]<mp) m[r,c] = 0;
                }
            } 
            m = m.HistEQ();
            mp = (int)m.MeanIfPixel((s) => {return (s>0);});
            m = m.NormalizeAllBetween(mp,255) * 255;

            m = m - vm;

            Tuple<int,int> centerBlob = new Tuple<int,int>(0,0);
            Tuple<int,int> center = new Tuple<int,int>(image.Width/2,image.Height/2);
            m = m.ToBinary(m.MeanIfPixel((s) => {return (s>0);})).GetLargestBlob(ref centerBlob);
            System.Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            
            double angle = Math.Atan2(centerBlob.Item1 - center.Item2,centerBlob.Item1 - center.Item1)*(180/Math.PI);
            double dist = Math.Abs(Math.Pow(centerBlob.Item1-centerBlob.Item2,2)+Math.Pow(center.Item1-center.Item2,2));
            List<dynamic> output = m.GetVectorizedMatrix().ToList();
            output.Add(angle);
            System.Console.WriteLine("image loaded");
        }

        public string AngleToCompass(double angle)
        {
            return ""
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
