using System.IO;

namespace GZipper
{
    /// <summary>
    /// Запись заархивированного массива байт в файл
    /// </summary>
    public class Writer : MessageQueue<byte[]>
    {
        private readonly Stream _writer;//Объект записи 
        private readonly object _lock;//Синхронизатор

        /// <summary>
        /// Конструткор
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="locker">Синхронизатор</param>
        public Writer(string fileName, object locker)
        {
            _writer = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            _lock = locker;
        }

        /// <summary>
        /// Запись в файл массива байт
        /// </summary>
        /// <param name="message">Сжатый массив байт</param>
        protected override void ProcessMessage(byte[] message)
        {
            _writer.Write(message, 0, message.Length);
        }

        /// <summary>
        /// Уничтожение экземпляра и освобождение ресурсов
        /// </summary>
        /// <param name="disposing">Флаг</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                _writer.Close( );
        }
    }
}
