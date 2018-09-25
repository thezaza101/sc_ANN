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
            int[] sArray = new int[]{1,2,3};
            SomeMethod(sArray[0]);
            SomeMethod(sArray[1]);
            SomeMethod(sArray[2]);
            SomeMethod(sArray.Skip(3).Select(z => (int?)z).FirstOrDefault());

            

            if (args.Length > 0)
            {
                ParseScript(args[0]);
            }
            while(Console.ReadLine()!="quit")
            {
                ParseCommand(Console.ReadLine());
            }
        }
        static void SomeMethod(int? s) => Console.WriteLine(s.HasValue);
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
