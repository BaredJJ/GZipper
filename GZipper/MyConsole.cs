using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace GZipper
{
    class MyConsole
    {
        public static Dictionary<string, string> ComanDictionary;//Библиотека команд

        static MyConsole( )
        {
            ComanDictionary = new Dictionary<string, string>
            {
                {"--help", "Show help information" },
                {"compress", "Compresiion file. In order to perform compression file, you must enter the command: compress: [path to file] [path to archive] and press Enter on keyboard" },
                {"decompress", "Decompress file. In order to perform decompression, you must enter the command: decompress: [path to archive] [path to file] and press Enter on keyboard" },
                {"--q", "Exit to programm" }
            };

        }

        public MyConsole(string[] args)
        {
            Run(args);
        }

        /// <summary>
        /// Выбор команды в зависимости от введеных данных
        /// </summary>
        /// <param name="args"></param>
        public void Run(string[] args)
        {
            Route route = new Route();
            switch (args[0].ToLower())
            {
                case "--help":
                    Help( );
                    break;
                case "compress":
                {
                    route.FileName = args[1];
                    route.ArchiveName = args[2];

                    if (route.IsFileName && route.IsArciveName)
                            Program.Run(route.FileName, route.ArchiveName, CompressionMode.Compress);

                    break;
                }
                case "decompress":
                {
                    route.ArchiveName = args[1];
                    route.FileName = args[2];

                    if(route.IsFileName && route.IsArciveName)
                        Program.Run(route.ArchiveName, route.FileName, CompressionMode.Decompress);

                    break;
                }
                case "--q": Environment.Exit(0); break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You entered incoorect command. If you want to see a list of commands you get --help.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
        }

        /// <summary>
        /// Помощь. Показ доступных команд
        /// </summary>
        private void Help( )
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('*', 50));

            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var command in ComanDictionary)
            {
                Console.WriteLine(command.Key + " - " + command.Value);
                Console.WriteLine( );
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('*', 50));

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
