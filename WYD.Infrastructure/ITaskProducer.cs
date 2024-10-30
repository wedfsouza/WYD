namespace WYD.Infrastructure;

public interface ITaskProducer
{
    void Write(Func<Task> task);
}