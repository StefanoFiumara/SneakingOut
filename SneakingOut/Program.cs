using System;
using System.IO;
using System.Linq;
using Fano.Logging.Core;

namespace SneakingOut
{
    internal class Program
    {
        public static ConsoleLogger Log;

        public static void Main(string[] args)
        {
            Log = new ConsoleLogger();

            //Receive the input file via cmd
            if (args.Length == 1)
            {
                string fileName = args[0];

                if (File.Exists(fileName))
                {
                    //Read all the lines and put each line in an array of strings
                    var input = File.ReadAllLines(fileName);

                    //pass the input to the SneakOut class to solve it.
                    new SneakOut(Log).Run(input.ToList());
                }
                else
                {
                    Log.Error($"Input file could not be found, please check your arguments: {args[0]}");
                }
            }
            else
            {
                Log.Warning("USAGE: SneakingOut.exe <input.txt>");
                Log.Warning("Please make sure you have the correct number of arguments.");
            }

            Console.Write("Press any key to close...");
            Console.ReadKey(intercept: true);
        }
    }
}
