namespace WYD.Infrastructure;

public class TaskProducer(TaskChannel taskChannel) : ITaskProducer
{
    public void Write(Func<Task> task) => taskChannel.Writer.TryWrite(task);
}