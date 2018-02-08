using System.Text;


namespace GZipper
{
    /// <summary>
    /// Вспомогательные методы.
    /// </summary>
    static class Helper
    {
        public static void PathToLower(this string original)
        {
            StringBuilder temp = new StringBuilder( );
            for (int i = 0; i < original.Length; ++i)
            {
                temp.Append(char.ToLower(original[i]));
            }

            original = temp.ToString( );
        }
    }
}
