using System.Threading.Channels;

namespace WYD.Infrastructure;

public class TaskChannel
{
    public TaskChannel()
    {
        var channel = Channel.CreateUnbounded<Func<Task>>(new UnboundedChannelOptions()
        {
            SingleReader = true,
        });
        
        Reader = channel.Reader;
        Writer = channel.Writer;
    }
    
    public ChannelReader<Func<Task>> Reader { get; }
    public ChannelWriter<Func<Task>> Writer { get; }
}