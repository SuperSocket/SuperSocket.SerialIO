﻿using System;
using System.IO.Ports;
using SuperSocket.Server.Abstractions;

namespace SuperSocket.SerialIO
{
    /// <summary>
    /// the serial port options
    /// </summary>
    public class SerialIOListenOptions : ListenOptions, ISerialIOEndPoint
    {
        public string PortName { get; set; }

        private byte _databits = 8;

        public int BaudRate { set; get; }

        public Parity Parity { set; get; } = Parity.None;

        public StopBits StopBits { set; get; } = StopBits.None;

        /// <summary>
        /// databits value ,,limit 5 to 8
        /// </summary>
        public byte Databits
        {
            set
            {
                if (value < 5 || value > 8)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "the value limit 5 to 8");
                }

                _databits = value;
            }

            get => _databits;
        }
    }
}
