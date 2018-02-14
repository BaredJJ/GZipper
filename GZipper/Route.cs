using System;
using System.Text.RegularExpressions;

namespace GZipper
{
    /// <summary>
    /// Путь
    /// </summary>
    public class Route
    {
        private readonly Regex _regex = new Regex(@"\w+\.\w+|[a-z]:[\\\w]+[\.\w]*");//Шаблон пути

        private string _fileName;
        private string _archivename;

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_regex.IsMatch(value))
                {
                    _fileName = _regex.Match(value).ToString();

                }
                else Console.WriteLine("Incorrect file route");
            }
        }

        /// <summary>
        /// Путь к архиву
        /// </summary>
        public string ArchiveName
        {
            get { return _archivename; }
            set
            {
                if (_regex.IsMatch(value))
                {
                    _archivename = _regex.Match(value).ToString();
                    
                }
                else Console.WriteLine("Incorrect arhcive route");               
            }
        }

        public bool IsFileName => (FileName!=null) && _regex.IsMatch(FileName);

        public bool IsArciveName => (ArchiveName != null) && _regex.IsMatch(ArchiveName);
    }
}
