using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGZipper
{
    [TestClass]
    public class MessageQueueTest
    {
        [TestMethod]
        public void SimpleConcat( )
        {
            using (var sc = new StringConcat())
            {
                sc.Enqueue("A");
                sc.Enqueue("BC");
                sc.Enqueue("D");
                sc.Stop();

                Assert.AreEqual("ABCD", sc.ToString());
            }
        }

        [TestMethod]
        public void Counter( )
        {
            using (var sc = new StringConcat( ))
            {
                sc.Enqueue("A");
                sc.Enqueue("BC");
                sc.Enqueue("D");
                sc.Stop( );

                Assert.AreEqual(3, sc.Count);
            }
        }

        [TestMethod]
        public void ConcatWithDelay()
        {
            using (var sc = new StringConcat(1000))
            {
                sc.Enqueue("A");
                sc.Enqueue("BC");
                System.Threading.Thread.Sleep(100);
                sc.Enqueue("D");
                sc.Stop( );

                Assert.AreEqual("ABCD", sc.ToString( ));
            }
        }

        [TestMethod]
        public void ConcatWithDelayInvert( )
        {
            using (var sc = new StringConcat(100))
            {
                sc.Enqueue("A");
                sc.Enqueue("BC");
                System.Threading.Thread.Sleep(1000);
                sc.Enqueue("D");
                sc.Stop( );

                Assert.AreEqual("ABCD", sc.ToString( ));
            }
        }
    }
}
