using System;
using helpers;

namespace cli
{
    class Program
    {
        static void Main(string[] args)
        {
            MatrixData<double> data = new MatrixData<double>("iris - Copy.Txt", false,' ');
            Console.WriteLine(data.Head(5,20));
            
           
            Console.WriteLine();
            Console.WriteLine("Normalizeing Data...");
            data.Normalize(0);
            data.Normalize(1);
            Console.WriteLine();

            Console.WriteLine(data.Head(5,20));
            Console.ReadLine();
        }
    }
}
