using System;
using System.Text.RegularExpressions;

namespace GZipper
{
    /// <summary>
    /// Путь
    /// </summary>
    class Route
    {
        private readonly Regex _regex = new Regex(@"[a-z,A-Z]:[\\\w]+[\.\w]*");//Шаблон пути

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FileName
        {
            get { return FileName; }
            set
            {
                if (_regex.IsMatch(value))
                {
                    Match match = _regex.Match(value);
                    ArchiveName = match.Success.ToString( );
                }
                else Console.WriteLine("Incorrect file route");
            }
        }

        /// <summary>
        /// Путь к архиву
        /// </summary>
        public string ArchiveName
        {
            get { return ArchiveName; }
            set
            {
                if (_regex.IsMatch(value))
                {
                    Match match = _regex.Match(value);
                    FileName = match.Success.ToString();
                }
                else Console.WriteLine("Incorrect arhcive route");               
            }
        }

        public bool IsFileName => _regex.IsMatch(FileName);

        public bool IsArciveName => _regex.IsMatch(ArchiveName);
    }
}
