using System;
using System.Linq;
using System.IO.Ports;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuperSocket.Channel;

namespace SuperSocket.SerialIO
{
    /// <summary>
    /// the serialport channel creator
    /// </summary>
    public class SerialIOChannelCreator : IChannelCreator
    {
        private ILogger _logger;
        private SerialPort _port = null;

        private Func<SerialPort, ValueTask<IChannel>> _channelFactory;
        
        public ListenOptions Options { get;  }

        /// <summary>
        /// if the serial port is open,then return true
        /// </summary>
        public bool IsRunning
        {
            get { return _port?.IsOpen ?? false; }
        }

        public event NewClientAcceptHandler NewClientAccepted;

        private SerialPort CreateSerialPort(ListenOptions options)
        {
            if (options is SerialIOListenOptions sioOptions)
            {
                return new SerialPort(sioOptions.PortName, sioOptions.BaudRate, sioOptions.Parity, sioOptions.Databits)
                {
                    StopBits = sioOptions.StopBits
                };
            }

            if(string.IsNullOrEmpty(options.Path) || !Uri.TryCreate(options.Path, UriKind.RelativeOrAbsolute, out Uri sioUri))
                throw new Exception("Invalid Path value in the ListenOptions.");

            var query = sioUri.Query
                .Split('&')
                .Select(x => x.Split('=', 2))
                .ToDictionary(x => x[0], x => x[1], StringComparer.OrdinalIgnoreCase);

            var portName = sioUri.Host;

            if (string.IsNullOrEmpty(portName))
                throw new Exception("PortName is required in the ListenOptions.");

            if (!query.TryGetValue("baudRate", out string baudRate) || string.IsNullOrEmpty(baudRate))
                throw new Exception("BaudRate is required in the ListenOptions.");

            if (!int.TryParse(baudRate, out int intBaudRate) || intBaudRate < 0)
                throw new Exception("BaudRate is invalid in the ListenOptions.");

            var parityValue = Parity.None;

            if (query.TryGetValue("parity", out string parity) && !string.IsNullOrEmpty(parity))
            {
                if (!Enum.TryParse(typeof(Parity), parity, true, out object parityParseValue))
                    throw new Exception("Parity is invalid in the ListenOptions.");
                
                parityValue = (Parity)parityParseValue;
            }

            var dataBitsValue = 8;

            if (query.TryGetValue("databits", out string databits) && !string.IsNullOrEmpty(databits))
            {
                if (!int.TryParse(databits, out int databitsParseValue))
                    throw new Exception("Databits is invalid in the ListenOptions.");

                dataBitsValue = databitsParseValue;
            }

            var stopBitsValue = StopBits.None;

            if (query.TryGetValue("stopbits", out string stopbits) && !string.IsNullOrEmpty(stopbits))
            {
                if (!Enum.TryParse(typeof(StopBits), stopbits, true, out object stopbitsParseValue))
                    throw new Exception("Stopbits is invalid in the ListenOptions.");
                
                stopBitsValue = (StopBits)stopbitsParseValue;
            }

            return new SerialPort(portName, intBaudRate, parityValue, dataBitsValue)
                {
                    StopBits = stopBitsValue
                };
        }

        public SerialIOChannelCreator(ListenOptions options, Func<SerialPort, ValueTask<IChannel>> channelFactory, ILogger logger)
        {
            _port = CreateSerialPort(options);

            Options = options;

            _channelFactory = channelFactory;
            _logger = logger;
        }

        public async Task<IChannel> CreateChannel(object connection)
        {
            return await _channelFactory((SerialPort) connection);
        }

        public bool Start()
        {
            
            try
            {
                _port.Open();

                //the serial port is diffent with the tcp, Client connections cannot be distinguished,and serial port never trigger the accept event
                //so the serial port only one channel and one client
                //so it need manual trigger the accept ,let the supersocket framework to force create the connection session
                NewClientAccepted?.Invoke( this,_channelFactory(_port).Result);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"serial port: {_port.PortName} open fail!");

                return false;
            }
        }

        public Task StopAsync()
        {
            //close the port when the server stop
            _port.Close();

            return Task.CompletedTask;
        }
    }
}