using System;
using helpers;

namespace cli
{
    class Program
    {
        static void Main(string[] args)
        {
            /*MatrixData data = new MatrixData("iris - Copy.Txt", false,' ');
            data.ChangeHeader(0,"Speal.Length");
            data.ChangeHeader(1,"Speal.Width");
            data.ChangeHeader(2,"Petal.Length");
            data.ChangeHeader(3,"Petal.Width");
            data.ChangeHeader(4,"Species");

            Console.WriteLine(data.Head(5,20));
           
            Console.WriteLine();
            Console.WriteLine("Normalizeing Data...");

            data.Normalize(0,NormalizationMethod.StandardScore);
            data.Normalize(1,NormalizationMethod.FeatureScalingStandardization);
            data.Normalize(2,NormalizationMethod.FeatureScalingMinMax);
            data.Normalize(3,NormalizationMethod.FeatureScalingMean);


            Console.WriteLine();

            Console.WriteLine(data.Head(20,20));

            Console.WriteLine();
            Console.WriteLine("Sorting Data (0, acen)...");
            Console.WriteLine();
            
            data.Sort(0);
            Console.WriteLine(data.Head(5,20));

            Console.WriteLine();
            Console.WriteLine("Sorting Data (0, decn)...");
            Console.WriteLine();

            data.Sort(0,false);
            Console.WriteLine(data.Head(5,20));

            Console.WriteLine();
            Console.WriteLine("Suffleing data...");
            Console.WriteLine();

            data.Suffle();
            Console.WriteLine(data.Head(5,20));

            Console.WriteLine();
            Console.WriteLine("Sorting Data (0, decn)...");
            Console.WriteLine();

            data.Sort(0,false);
            Console.WriteLine(data.Head(5,20));

            data.WriteCSV("test.csv");

            MatrixData data = new MatrixData("test.csv", false);
            //System.Console.WriteLine(data.Head(10));

            //System.Console.WriteLine("CopyData(0,0) test");
            //MatrixData data2 = data.CopyData(0,0);
            //System.Console.WriteLine(data.Head(10));
            
            System.Console.WriteLine("Spitting the data in half..");
            MatrixData secondHalf = data.SplitData(5,0);
            System.Console.WriteLine(data.Head());
            System.Console.WriteLine(secondHalf.Head());
            MatrixData secondColHalf = data.SplitData(0,5);
            System.Console.WriteLine(data.Head());
            System.Console.WriteLine(secondColHalf.Head());*/

            MatrixData data = new MatrixData("task2z11.txt", false,' ');

            Console.WriteLine(data.Head(10,20));

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();

            MatrixData exemplarData = data.GetExemplar(2,2,1);
            Console.WriteLine(exemplarData.ToString());
            var test = data.IndexOf(0,data[1,0]);



            Console.ReadLine();

        }
    }
}
