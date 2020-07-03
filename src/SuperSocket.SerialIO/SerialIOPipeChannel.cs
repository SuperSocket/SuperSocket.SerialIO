using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;

namespace SuperSocket.SerialIO
{
    public class SerialIOPipeChannel<TPackageInfo> : PipeChannel<TPackageInfo>
    {
        private SerialPort _serialPort = null;

        public SerialIOPipeChannel(SerialPort serialPort, IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options)
            : base(pipelineFilter, options)
        {
            _serialPort = serialPort;
        }

        protected override void Close()
        {
            //don't close the serial port in the channel, only close in creator's StopAsync
            //throw new NotImplementedException();
        }

        protected override async ValueTask<int> FillPipeWithDataAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            return await ReceiveAsync(memory, cancellationToken);
        }

        private async ValueTask<int> ReceiveAsync(Memory<byte> memory, CancellationToken cancellationToken)
        {
            //read the data from serialport by async 
            return await _serialPort.BaseStream.ReadAsync(memory, cancellationToken);
        }

        protected override async ValueTask<int> SendOverIOAsync(ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
        {
            if (buffer.IsSingleSegment)
            {
                var segment = GetArrayByMemory(buffer.First);

                await _serialPort.BaseStream.WriteAsync(segment.Array, segment.Offset, segment.Count);

                return segment.Count;
            }

            var count = 0;

            foreach (var piece in buffer)
            {
                cancellationToken.ThrowIfCancellationRequested();

                //write the data to serial port stream
                await _serialPort.BaseStream.WriteAsync(piece, cancellationToken);

                count += piece.Length;
            }

            return count;
        }
    }
}
