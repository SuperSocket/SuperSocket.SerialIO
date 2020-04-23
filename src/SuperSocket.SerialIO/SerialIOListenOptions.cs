using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace SuperSocket.SerialIO
{
    public class SerialIOListenOptions:ListenOptions
    {
        public int BaudRate { set; get; }

        public Parity Parity { set; get; }
    }
}
