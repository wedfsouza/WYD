using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Net.Sockets;

namespace WYD.Network;

public sealed class Session : ISession
{
    private readonly Socket _socket;
    private readonly IProtocol _protocol;
    private readonly ILogger<Session> _logger;
    private readonly ArrayPool<byte> _bufferPool = ArrayPool<byte>.Shared;
    private byte[] _receiveBuffer;
    
    public Session(Socket socket, IProtocol protocol, ILoggerFactory loggerFactory)
    {
        _socket = socket;
        _protocol = protocol;
        _logger = loggerFactory.CreateLogger<Session>();
        _receiveBuffer = _bufferPool.Rent(256);
    }

    public async Task RunAsync()
    {
        try
        {
            if (!await ParseHandshakeAsync())
            {
                _logger.LogInformation("Invalid handshake received. Closing session.");
                return;
            }

            await _protocol.OnConnectedAsync(this);

            await HandleIncomingPacketsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred");
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    private async Task DisconnectAsync()
    {
        try
        {
            _socket.Shutdown(SocketShutdown.Both);
        }
        catch (SocketException ex)
        {
            _logger.LogError(ex, "Error shutting down socket");
        }

        _socket.Close();

        if (_receiveBuffer is not null)
        {
            _bufferPool.Return(_receiveBuffer);
        }

        await _protocol.OnDisconnectedAsync(this);
    }

    private async Task HandleIncomingPacketsAsync()
    {
        while (true)
        {
            var packetBuffer = await ReadPacketAsync();

            // TODO: Decrypt packet
        }
    }

    private async Task<bool> ParseHandshakeAsync()
    {
        const int HandshakeLength = 4;
        const uint HandshakeCode = 0x1F11F311;

        await ReceiveAsync(HandshakeLength);

        var handshake = BitConverter.ToUInt32(_receiveBuffer, 0);
        return handshake == HandshakeCode;
    }

    private async Task<byte[]> ReadPacketAsync()
    {
        const int HeaderLength = 2;
        const int MinimumPacketSize = 12;

        await ReceiveAsync(HeaderLength);

        var packetSize = BitConverter.ToUInt16(_receiveBuffer, 0);
        if (packetSize < MinimumPacketSize)
        {
            throw new InvalidOperationException("Packet size is smaller than the minimum packet size");
        }

        await ReceiveAsync(packetSize - HeaderLength, offset: HeaderLength);

        var packetBuffer = new byte[packetSize];
        Buffer.BlockCopy(_receiveBuffer, 0, packetBuffer, 0, packetSize);

        return packetBuffer;
    }

    private async Task ReceiveAsync(int size, int offset = 0)
    {
        EnsureBufferCapacity(size);

        int totalBytesReceived = 0;

        while (totalBytesReceived < size)
        {
            var bytesReceived = await _socket.ReceiveAsync(new ArraySegment<byte>(_receiveBuffer, offset + totalBytesReceived, size - totalBytesReceived));
            if (bytesReceived == 0)
            {
                throw new InvalidOperationException("Socket closed or error occurred during receive");
            }

            totalBytesReceived += bytesReceived;
        }
    }

    private void EnsureBufferCapacity(int size)
    {
        if (_receiveBuffer.Length < size)
        {
            _bufferPool.Return(_receiveBuffer);

            _receiveBuffer = _bufferPool.Rent(size);
        }
    }
}
