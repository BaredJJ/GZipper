using GZipper;
using System.IO;
using System.IO.Compression;

namespace TestGZipper
{
    class TestWriter : MessageQueue<byte[]>
    {

        public void Compress( string path1, string path2, CompressionMode mode )
        {
            using (FileStream input =
    new FileStream(path1, FileMode.Open, FileAccess.Read))
            {
                    using (FileStream output = new FileStream(path2, FileMode.Create, FileAccess.Write))
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
            }
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

        public void ReadFile(string path)
        {
            if(File.Exists(path))
            {
                using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    //TODO
                }
            }
        }

        protected override void ProcessMessage(byte[] message)
        {
            
        }
    }
}
