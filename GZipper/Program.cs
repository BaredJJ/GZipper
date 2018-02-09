using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace GZipper
{
    static class Program
    {
        public static void Run(string input, string output, CompressionMode mode, int bufferSize = 1024000)
        {
            if (!File.Exists(input))
                return;

            var locker = new object();
            var writer = new Writer(output, locker);
            var comp = new Compressor(writer, mode);

            using (var fs = new FileStream(input, FileMode.Open, FileAccess.Read))
            {
                for (long pos = 0; pos < fs.Length; pos += bufferSize)
                {
                    fs.Position = pos;
                    var buffer = new byte[Math.Min(bufferSize, fs.Length - pos)];

                    lock (locker)
                    {
                        fs.Read(buffer, 0, buffer.Length);
                        comp.Enqueue(buffer);
                    }
                }
            }
        }

        //Временно
        public static string[] GetArrayString(this string source)=>Regex.Split(source, " ");


        static void Main(string[] args)
        {
            Console.Write($"Please enter the path of the file to the template [File name] [Archive name]:");
            string path = Console.ReadLine();
            path.PathToLower();
            string[] args1 = path.GetArrayString();

            new MyConsole(args1);
            Console.WriteLine("Opreration completed!");
            Console.ReadKey();
            System.Environment.Exit(0);
        }
    }
}
