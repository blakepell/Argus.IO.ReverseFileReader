using System;
using System.Collections.Generic;
using System.IO;

namespace Tail
{
    /// <summary>
    /// A tail program to view the last set of lines in a specified file.
    /// </summary>
    class Program
    {
        //*********************************************************************************************************************
        //
        //           Program:  Tail
        //      Organization:  http://www.blakepell.com  
        //      Initial Date:  04/06/2019
        //     Last Modified:  04/06/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************      

        static void Main(string[] args)
        {
            int linesToFetch = 10;
            string filePath = "";

            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: tail <filename> <optional:number of lines>");
                return;
            }

            if (args.Length == 1)
            {
                filePath = args[0];
            }
            else if (args.Length == 2)
            {
                filePath = args[0];

                try
                {
                    linesToFetch = int.Parse(args[1]);
                }
                catch (Exception)
                {
                    Console.WriteLine("The second argument if provided must be the number of lines you want to echo out.");
                    return;
                }
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"The file '{filePath}' was not found or is inaccessible.");
                return;
            }

            // For this sample program I'm making an assumption all files will having \r\n line
            // endings.  Perhaps a good upgrade to the library would be for it to autodetect the
            // line endings of the file.  We'll only read in the last X lines, then we will reverse
            // those lines so they're from the end of the file but in sequestial first to last order.
            using (var reader = new Argus.IO.ReverseFileReader(filePath))
            {
                var lines = new List<string>();
                int counter = 0;

                while (!reader.StartOfFile)
                {
                    counter++;
                    lines.Add(reader.ReadLine());

                    if (counter > linesToFetch)
                    {
                        break;
                    }
                }

                lines.Reverse();

                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }

            }
        }
    }
}
