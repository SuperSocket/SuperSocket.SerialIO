using System;
using System.IO.Ports;
using System.Linq;

namespace SuperSocket.SerialIO
{
    public interface ISerialIOEndPoint
    {
        string PortName { get; set; }

        byte Databits { get; set; }

        int BaudRate { set; get; }

        Parity Parity { set; get; }

        StopBits StopBits { set; get; }
    }

    public class SerialIOEndPoint : ISerialIOEndPoint
    {
        public string PortName { get; set; }

        public byte Databits { get; set; }

        public int BaudRate { set; get; }

        public Parity Parity { set; get; }

        public StopBits StopBits { set; get; }

        public SerialIOEndPoint()
        {

        }

        public SerialIOEndPoint(string uri)
            : this(new Uri(uri))
        {
            
        }

        public SerialIOEndPoint(Uri uri)
        {
            var query = uri.Query.TrimStart('?')
                .Split('&')
                .Select(x => x.Split('=', 2))
                .ToDictionary(x => x[0], x => x[1], StringComparer.OrdinalIgnoreCase);

            PortName = uri.Host;

            if (string.IsNullOrEmpty(PortName))
                throw new Exception("PortName is required in the ListenOptions.");

            if (!query.TryGetValue("baudRate", out string baudRate) || string.IsNullOrEmpty(baudRate))
                throw new Exception("BaudRate is required in the ListenOptions.");

            if (!int.TryParse(baudRate, out int intBaudRate) || intBaudRate < 0)
                throw new Exception("BaudRate is invalid in the ListenOptions.");

            BaudRate = intBaudRate;

            var parityValue = Parity.None;

            if (query.TryGetValue("parity", out string parity) && !string.IsNullOrEmpty(parity))
            {
                if (!Enum.TryParse(typeof(Parity), parity, true, out object parityParseValue))
                    throw new Exception("Parity is invalid in the ListenOptions.");
                
                parityValue = (Parity)parityParseValue;
            }

            Parity = parityValue;

            var dataBitsValue = 8;

            if (query.TryGetValue("databits", out string databits) && !string.IsNullOrEmpty(databits))
            {
                if (!int.TryParse(databits, out int databitsParseValue))
                    throw new Exception("Databits is invalid in the ListenOptions.");

                dataBitsValue = databitsParseValue;
            }

            Databits = (byte)dataBitsValue;


            var stopBitsValue = StopBits.None;

            if (query.TryGetValue("stopbits", out string stopbits) && !string.IsNullOrEmpty(stopbits))
            {
                if (!Enum.TryParse(typeof(StopBits), stopbits, true, out object stopbitsParseValue))
                    throw new Exception("Stopbits is invalid in the ListenOptions.");
                
                stopBitsValue = (StopBits)stopbitsParseValue;
            }

            StopBits = stopBitsValue;
        }
    }
}
