﻿using System;
using helpers;

namespace cli
{
    class Program
    {
        static void Main(string[] args)
        {
            MatrixData data = new MatrixData("iris - Copy.Txt", false,' ');
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

            
            Console.ReadLine();
        }
    }
}
