using System.Buffers.Binary;

namespace WYD.Network;

public sealed class IncomingPacket
{
    private readonly ReadOnlyMemory<byte> _buffer;
    private int _position;
    
    public IncomingPacket(ReadOnlyMemory<byte> buffer)
    {
        if (buffer.Length < PacketConstants.MinimumPacketSize)
        {
            throw new ArgumentException("Buffer size is smaller than minimum packet size", nameof(buffer));
        }
        
        _buffer = buffer;
        _position = 0;

        _ = ReadBytes(4);
        Code = ReadUInt16();
        ClientId = ReadInt16();
        Timestamp = ReadUInt32();
    }

    public ushort Code { get; }
    public short ClientId { get; }
    public uint Timestamp { get; }

    public ReadOnlySpan<byte> ReadBytes(int length) => ReadSpan(length);

    public short ReadInt16()
    {
        var span = ReadSpan(sizeof(short));
        return BinaryPrimitives.ReadInt16LittleEndian(span);
    }

    public ushort ReadUInt16()
    {
        var span = ReadSpan(sizeof(ushort));
        return BinaryPrimitives.ReadUInt16LittleEndian(span);
    }
    
    public int ReadInt32()
    {
        var span = ReadSpan(sizeof(int));
        return BinaryPrimitives.ReadInt32LittleEndian(span);
    }
    
    public uint ReadUInt32()
    {
        var span = ReadSpan(sizeof(uint));
        return BinaryPrimitives.ReadUInt32LittleEndian(span);
    }
    
    private ReadOnlySpan<byte> ReadSpan(int length)
    {
        if (_position + length > _buffer.Length)
        {
            throw new InvalidOperationException("Attempt to read more bytes than the end of the buffer");
        }
        
        var span = _buffer.Span.Slice(_position, length);
        _position += length;
        return span;
    }
}