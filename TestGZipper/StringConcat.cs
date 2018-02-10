using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GZipper;

namespace TestGZipper
{
    class StringConcat: MessageQueue<string>
    {
        readonly StringBuilder _sb = new StringBuilder();
        private readonly int _wait;
        private volatile int _cnt = 0;

        public StringConcat(int wait=0)
        {
            _wait = wait;
        }

        protected override void ProcessMessage(string message)
        {
            if (_wait != 0)
                System.Threading.Thread.Sleep(_wait);

            _sb.Append(message);
            _cnt++;
        }

        public int Count => _cnt;

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
