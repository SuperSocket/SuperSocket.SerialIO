using System;
using System.Buffers;
using System.Threading.Tasks;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;

namespace SuperSocket.SerialIO
{
    public class SerialIOPipeChannel<TPackageInfo> : PipeChannel<TPackageInfo>
        where TPackageInfo : class
    {
        public SerialIOPipeChannel(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options)
            : base(pipelineFilter, options)
        {

        }
        public override void Close()
        {
            throw new NotImplementedException();
        }

        protected override ValueTask<int> FillPipeWithDataAsync(Memory<byte> memory)
        {
            throw new NotImplementedException();
        }

        protected override ValueTask<int> SendOverIOAsync(ReadOnlySequence<byte> buffer)
        {
            throw new NotImplementedException();
        }
    }
}
