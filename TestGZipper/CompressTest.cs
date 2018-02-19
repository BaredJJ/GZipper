
using GZipper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.IO.Compression;

namespace TestGZipper
{
    [TestClass]
    public class CompressTest
    {
        private Random _random = new Random( );

        private byte[] GetByte()
        {            
            byte[] array = new byte[10];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)_random.Next(0, 254);
            }
            return array;
        }

        [TestMethod]
        public void AllFileCompress()
        {
            string arcive = @"123.gz";
            string file = @"1.txt";
            string newFile = @"12.txt";
            using (TestWriter test = new TestWriter())
            {
                test.Compress(file, arcive, CompressionMode.Compress);
                test.Compress(arcive, newFile, CompressionMode.Decompress);

                Assert.IsTrue(test.Check(newFile));
                File.Delete(arcive);
                File.Delete(newFile);
            }
        }

        [TestMethod]
        public void AllMemoryCompress()
        {
            var tw = new TestWriter( );
            byte[] array = this.GetByte( );

            var compressed = tw.Compress(array, CompressionMode.Compress);
            var decompressed = tw.Compress(compressed, CompressionMode.Decompress);

            Assert.IsTrue(tw.Check(array, decompressed));
        }

        [TestMethod]
        public void ByteArray()
        {
            var tw = new TestWriter( );
            //string archive = @"123.gz";
            byte[] array = this.GetByte( );
            var ms = new MemoryStream(array);

            //if (File.Exists(archive))
            //    File.Delete(archive);

            var compressed = tw.Compress(array, CompressionMode.Compress);
            using (Writer writer = new Writer(ms/*archive*/, new object( )))
            {
                using (var compressor = new Compressor(writer, CompressionMode.Compress))
                {
                    compressor.Enqueue(array);
                    compressor.Stop( );
                }

                writer.Stop( );
            }

            Assert.IsTrue(tw.Check(ms.ToArray(), compressed));
        }


        [TestMethod]
        public void ByteArrayDecompress()
        {
            var tw = new TestWriter( );
            //string archive = @"123.gz";
            byte[] array = this.GetByte( );
            var ms = new MemoryStream(array);

            //if (File.Exists(archive))
            //    File.Delete(archive);

            var compressed = tw.Compress(array, CompressionMode.Compress);
            using (Writer writer = new Writer(ms/*archive*/, new object( )))
            {
                using (var compressor = new Compressor(writer, CompressionMode.Decompress))
                {
                    compressor.Enqueue(compressed);
                    compressor.Stop( );
                }

                writer.Stop( );
            }

            Assert.IsTrue(tw.Check(ms.ToArray()/*File.ReadAllBytes(archive)*/, array));
        }

        [TestMethod]
        public void TestFullCompress()
        {
            var tw = new TestWriter( );
            byte[] array = this.GetByte( );

            var compressed = tw.Compress(array, CompressionMode.Compress);

            using (var ms = new MemoryStream( ))
            {
                using (var writer = new Writer(ms, new object( )))
                {
                    using (var comp = new Compressor(writer, CompressionMode.Compress))
                    {
                        for (int shift = 0; shift < array.Length; shift += array.Length / 10) 
                        {
                            var buf = new byte[array.Length / 10];

                            for (int i = 0; i < buf.Length; i++)
                                buf[i] = array[shift + i];

                            comp.Enqueue(buf);
                        }

                        comp.Stop( );
                    }

                    writer.Stop( );
                }

                Assert.IsTrue(tw.Check(ms.ToArray( ), compressed));
            }
        }

        [TestMethod]
        public void TestWriteRead()
        {
            var tw = new TestWriter( );
            byte[] array = this.GetByte( );

            var compress = tw.Compress(array, CompressionMode.Compress, 1);
            var decompressor = tw.Decompress(compress, CompressionMode.Decompress);

            Assert.IsTrue(tw.Check(array, decompressor));
        }

        [TestMethod]
        public void BlockWrite()
        {
            var tw = new TestWriter( );
            byte[] array = this.GetByte( );

            var compress = tw.Compress(array, CompressionMode.Compress, 1);
            byte[] twoCompress = new byte[compress.Length];
            using (var ms = new MemoryStream( ))
            {
                using(GZipStream packer = new GZipStream(ms, CompressionMode.Compress))
                for (int i = 0, pos = 0; i < array.Length; i += array.Length / 10)
                {
                    byte[] temp = new byte[Math.Min(array.Length / 10, array.Length - i)];
                    Array.Copy(array, i, temp, 0, temp.Length);

                    tw.Compress(packer, temp);
                    //var tp = tw.Compress(temp, CompressionMode.Compress, 1);
                    //for(int j = 0; j < tp.Length; j++)
                    //{
                    //    twoCompress[pos] = tp[j];
                    //    pos++;
                    //}
                }
                twoCompress = ms.ToArray( );
            }

            Assert.IsTrue(tw.Check(compress, twoCompress));
        }

        [TestMethod]
        public void BlockDecompress()
        {
            var tw = new TestWriter( );
            byte[] array = this.GetByte( );
            byte[] decompress = new byte[array.Length];

            var compress = tw.Compress(array, CompressionMode.Compress, 1);
            using (var ms = new MemoryStream(compress ))
            {
                using (GZipStream packer = new GZipStream(ms, CompressionMode.Decompress))
                {
                    for (int i = 0, pos =0; i < compress.Length; i += compress.Length / 10)
                    {
                        byte[] temp = new byte[Math.Min(compress.Length / 10, compress.Length - i)];
                        Array.Copy(array, i, temp, 0, temp.Length);

                        var tp = tw.Decompress(temp, CompressionMode.Compress);
                        for (int j = 0; j < tp.Length; j++)
                        {
                            decompress[pos] = tp[j];
                            pos++;
                        }
                    }
                }
            }

            Assert.IsTrue(tw.Check(array, decompress));
        }
    }
}
