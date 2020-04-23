using System;
using System.Buffers;
using System.IO.Ports;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuperSocket.Channel;

namespace SuperSocket.SerialIO
{
    public class SerialIOChannelCreator : IChannelCreator
    {
        private ILogger _logger;
        private SerialPort _port = null;

        private Func<SerialPort, ValueTask<IChannel>> _channelFactory;
        
        public ListenOptions Options { get;  }

        public bool IsRunning
        {
            get { return _port?.IsOpen ?? false; }
        }

        public event NewClientAcceptHandler NewClientAccepted;

        public SerialIOChannelCreator(SerialIOListenOptions options, Func<SerialPort, ValueTask<IChannel>> channelFactory, ILogger logger)
        {
            //if (!options.Path.StartsWith("sio://"))
            //{
            //    throw new ArgumentOutOfRangeException("Path", "����ֻ��ʹ��sio://��ͷ���ַ���");
            //}


            _port=new SerialPort(options.Ip,options.BaudRate,options.Parity);
            
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

                NewClientAccepted?.Invoke( this,_channelFactory(_port).Result);

                return true;
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error,$"����{_port.PortName}��ʧ��");

                return false;
            }
        }

        public async Task StopAsync()
        {
            _port.Close();

        }
    }
}