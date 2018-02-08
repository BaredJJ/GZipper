using System;
using System.IO;
using System.IO.Compression;

namespace GZipper
{
    static class Program
    {
        public static void Run(string input, string output, CompressionMode mode, int bufferSize = 1024)
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

        static void Main(string[] args)
        {
            new MyConsole(args);
            Console.ReadKey();
        }
    }
}
