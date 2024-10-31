namespace WYD.Network;

public interface ISession
{
    int Id { get; }
    void AssignSessionId(int id);
    Task RunAsync();
    Task DisconnectAsync();
}
