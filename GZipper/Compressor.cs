using System.IO;
using System.IO.Compression;

namespace GZipper
{
    class Compressor : MessageQueue<byte[]>
    {
        private readonly Writer _writer;//Экземпляр класса для записи сжатого архива
        private readonly CompressionMode _mode;//Режим архивирования/разархивирования

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="mode"></param>
        public Compressor(Writer writer, CompressionMode mode)
        {
            _writer = writer;
            _mode = mode;
        }

        /// <summary>
        /// Сжатие архива байт
        /// </summary>
        /// <param name="message">Массив байт</param>
        protected override void ProcessMessage(byte[] message)
        {
            using (var ms = new MemoryStream( ))
            {
                using (var gzip = new GZipStream(ms, _mode))
                {
                    gzip.Write(message, 0, message.Length);

                    _writer.Enqueue(ms.ToArray( ));
                }
            }
        }
    }
}
