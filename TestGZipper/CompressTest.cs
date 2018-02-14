
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.IO.Compression;

namespace TestGZipper
{
    [TestClass]
    public class CompressTest
    {
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
        public void ByteArray()
        {
            Random random = new Random( );
            byte[] array = new byte[10];
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)random.Next(0, 254);
            }


        }
    }
}
