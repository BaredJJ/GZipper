using GZipper;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace TestGZipper
{

    class TestWriter : MessageQueue<byte[]>
    {

        public void Compress(string inFile, string outFile, CompressionMode mode)
        {
            using (var inStream = new FileStream(inFile, FileMode.Open, FileAccess.Read))
            {
                using (var outStream = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                {
                    Compress(inStream, outStream, mode);
                }
            }
        }

        public void Compress(GZipStream packer, byte[] input)
        {
            packer.Write(input, 0, input.Length);
        }

        public byte[] Decompress(GZipStream packer, byte[] output)
        {
                List<byte> myByte = new List<byte>( );

                for(int i = 0; i < packer.Length; i++)
                {
                    myByte.Add((byte)packer.ReadByte( ));
                }

                byte[] array = new byte[myByte.Count];
                for (int i = 0; i < myByte.Count; i++)
                    array[i] = myByte[i];

                return array;
        }

        public byte[] Compress(byte[] input, CompressionMode mode, int i = 0)
        {
            using (var ms = new MemoryStream( ))
            {
                using (GZipStream packer = new GZipStream(ms, mode))
                {
                    packer.Write(input, 0, input.Length);

                }
                return ms.ToArray( );
            }
        }

        public byte[] Decompress(byte[] value, CompressionMode mode)
        {
            using (var ms = new MemoryStream(value))
            {
                byte[] input = new byte[1000];
                using (GZipStream packer = new GZipStream(ms, mode))
                {
                    packer.Read(input, 0, input.Length);
                }
                return input;
            }
        }

        public void Compress( Stream input, Stream output, CompressionMode mode )
        {
            if (mode == CompressionMode.Compress)
                using (GZipStream packer = new GZipStream(output, mode))
                {
                    input.CopyTo(packer);
                }
            else
                using (GZipStream packer = new GZipStream(input, mode))
                {
                    packer.CopyTo(output);
                }
        }

        public byte[] Compress(byte[] input, CompressionMode mode)
        {
            using (var inStream = new MemoryStream(input))
            {
                using (var outStream = new MemoryStream( ))
                {
                    this.Compress(inStream, outStream, mode);

                    return outStream.ToArray( );
                }
            }
        }

        public bool Check(byte[] a1, byte[] a2)
        {
            if (a1 == null || a1 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                    return false;
            }

            return true;
        }

        public bool Check(string path)
        {
            using (FileStream writer1 = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (FileStream writer2 = new FileStream(@"1.txt", FileMode.Open, FileAccess.Read))
                {
                    if (writer1.Length == writer2.Length)
                    {
                        for(long i = 0; i < writer1.Length; i+=2)
                        {
                            if (writer1.ReadByte( ) != writer2.ReadByte( ))
                                return false;
                        }
                    }
                    else return false;
                }
            }
            return true;
        }

        public byte[] ReadFile(string path)
        {
            byte[] array = new byte[0];
            if(File.Exists(path))
            {
                using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    array = new byte[reader.Length / 2];
                    for (long i = 0; i < reader.Length; i+=2)
                    {
                        array[i / 2] = (byte)reader.ReadByte( );
                    }
                }
            }
            return array;
        }

        protected override void ProcessMessage(byte[] message)
        {
            
        }
    }
}
