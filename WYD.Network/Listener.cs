using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace WYD.Network;

public abstract class Listener : IListener
{
    private readonly Socket _socket;
    private readonly IProtocol _protocol;
    private readonly ILoggerFactory _loggerFactory;

    public Listener(string ipAddress, int port, IProtocol protocol, ILoggerFactory loggerFactory)
    {
        _protocol = protocol;
        _loggerFactory = loggerFactory;

        var localEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Bind(localEndPoint);
        _socket.Listen();
    }

    public void Start() => Task.Run(AcceptConnectionsAsync);

    private async Task AcceptConnectionsAsync()
    {
        while (true)
        {
            var acceptedSocket = await _socket.AcceptAsync();
            var session = new Session(acceptedSocket, _protocol, _loggerFactory);

            _ = Task.Run(session.RunAsync);
        }
    }
}
