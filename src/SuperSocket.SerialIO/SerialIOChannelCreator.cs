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
            var sioEndpoint = options as ISerialIOEndPoint;

            if (sioEndpoint == null)
            {
                if(string.IsNullOrEmpty(options.Path))
                    throw new Exception("Invalid Path value in the ListenOptions.");

                sioEndpoint = new SerialIOEndPoint(options.Path);
            }

            return new SerialPort(sioEndpoint.PortName, sioEndpoint.BaudRate, sioEndpoint.Parity, sioEndpoint.Databits)
                {
                    StopBits = sioEndpoint.StopBits
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