using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicShuffler
{
    class Program
    {
        static void Main(string[] args)
        {
            string source, destination, error;

            (source, destination, error) = ParseArgs(args);

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine(error);
                return;
            }

            if (IsValidArgs(source, destination))
            {
                Shuffle(source, destination);
            }
        }



        static (string source, string destination, string error) ParseArgs(string[] args)
        {
            if (args.Length == 2)
            {
                if (args[0] != "-s")
                {
                    return (null, null, "Please specify source and destination directory.");
                }
                return (args[1], args[1], null);
            }
            else if (args.Length == 4)
            {
                if (args[0] == "-s")
                {
                    return (args[1], args[3], null);
                }
                else
                {
                    return (args[3], args[1], null);
                }
            }
            return (null, null, "Please specify source and destination directory.");
        }

        private static bool IsValidArgs(string source, string destination)
        {
            if (!Directory.Exists(source))
            {
                Console.WriteLine($"Directory {source} does not exist");
                return false;
            }

            if (!Directory.Exists(destination))
            {
                try
                {
                    Directory.CreateDirectory(destination);
                    Console.WriteLine($"Directory {destination} was created.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not create directory {destination}. Error: {ex.Message}");
                    return false;
                }
            }

            return true;
        }
        private static void Shuffle(string source, string destination)
        {
            var files = Directory.GetFiles(source);
            var padding = files.Length.ToString().Length;
            string numberFormat = $"D{padding}";
            var numbersList = new List<int>(Enumerable.Range(1, files.Length));
            var rand = new Random();
            for (int i = 0; i < files.Length; i++)
            {
                var nextNum = rand.Next(numbersList.Count - 1);
                var numberedName = $"{numbersList[nextNum].ToString(numberFormat)} - {Path.GetFileName(files[i])}";
                File.Copy(files[i], Path.Combine(destination, numberedName));
                numbersList.RemoveAt(nextNum);
            }
        }
    }
}
