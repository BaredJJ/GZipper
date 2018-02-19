using System.IO;
using System.IO.Compression;

namespace GZipper
{
    public class Compressor : MessageQueue<byte[]>
    {
        private readonly Writer _writer;//Экземпляр класса для записи сжатого архива
        private readonly CompressionMode _mode;//Режим архивирования/разархивирования
        private readonly GZipStream _gzip;
        private readonly MemoryStream _ms;

        /// <param name="writer"></param>
        /// <param name="mode"></param>
        public Compressor(Writer writer, CompressionMode mode)
        {
            _writer = writer;
            _mode = mode;
            _ms = new MemoryStream( );
            _gzip = new GZipStream(_ms, mode);
        }

        // TODO kill streams
        protected override void Dispose(bool disposing) => base.Dispose(disposing);

        /// <summary>
        /// Сжатие архива байт
        /// </summary>
        /// <param name="message">Массив байт</param>
        protected override void ProcessMessage(byte[] message)
        {
            var pos = _ms.Position;
            _gzip.Write(message, 0, message.Length);

            var buffer = new byte[_ms.Position - pos];
            _ms.Position = pos;
            _ms.Read(buffer, 0, buffer.Length);
            _ms.Position = pos + buffer.Length;
            _writer.Enqueue(buffer);
            /*using (var input = new MemoryStream(message))
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
            }*/
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
