using System.IO;
using System.IO.Compression;

namespace GZipper
{
    public class Compressor : MessageQueue<byte[]>
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
            using (var input = new MemoryStream(message))
            {
                using (var output = new MemoryStream( ))
                {
                    if (_mode == CompressionMode.Compress)
                        using (var gzip = new GZipStream(output, _mode))
                            input.CopyTo(gzip);
                    else using (var gzip = new GZipStream(input, _mode))
                            gzip.CopyTo(output);
                    _writer.Enqueue(output.ToArray( ));
                }
            }
            //using (var ms = new MemoryStream( ))
            //{
            //    if (_mode == CompressionMode.Compress)
            //        using (var gzip = new GZipStream(ms, _mode))
            //        {
            //            gzip.Write(message, 0, message.Length);
            //            //_writer.Enqueue(ms.ToArray( ));
            //        }
            //    else
            //    {
            //        using (var gzip = new GZipStream(new MemoryStream(message), _mode))
            //        {

            //        }
                       
            //    }
            //}
        }
    }
}
