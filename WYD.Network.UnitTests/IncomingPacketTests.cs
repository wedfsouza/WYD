using System.Buffers.Binary;

namespace WYD.Network.UnitTests;

public class IncomingPacketTests
{
    [Fact]
    public void IncomingPacket_WithPacketSizeBelowMinimum_ThrowsException()
    {
        var buffer = new byte[1];
        var exception = Assert.Throws<ArgumentException>(() => new IncomingPacket(buffer));
        Assert.Equal("Buffer size is smaller than minimum packet size (Parameter 'buffer')", exception.Message);
    }

    [Fact]
    public void IncomingPacket_WhenAttemptToReadMoreBytesThanBufferSize_ThrowsException()
    {
        var buffer = new byte[12];
        var incomingPacket = new IncomingPacket(buffer);
        
        var exception = Assert.Throws<InvalidOperationException>(() => incomingPacket.ReadBytes(100));
        Assert.Equal("Attempt to read more bytes than the end of the buffer", exception.Message);
    }

    [Fact]
    public void IncomingPacket_WhenPacketIsValid_ReadsPacketHeaderData()
    {
        var buffer = new byte[12];
        var span = new Span<byte>(buffer);
        
        BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(4, 2), 525);
        BinaryPrimitives.WriteInt16LittleEndian(span.Slice(6, 2), 1);
        BinaryPrimitives.WriteUInt32LittleEndian(span.Slice(8, 4), 40746305);
        
        var incomingPacket = new IncomingPacket(buffer);
        
        Assert.Equal(525, incomingPacket.Code);
        Assert.Equal(1, incomingPacket.ClientId);
        Assert.Equal(40746305, (int)incomingPacket.Timestamp);
    }
}