using System.Buffers.Binary;
using Moq;
using WYD.Network.Criptography;

namespace WYD.Network.UnitTests;

public class PacketCriptographyTests
{
    [Fact]
    public void Encrypt_SetsPacketSizeInBuffer()
    {
        var packet = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x02, 0x0D, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
        var keywordGenerator = new Mock<IKeywordGenerator>();

        PacketCriptography.Encrypt(packet, keywordGenerator.Object);

        var packetSize = BinaryPrimitives.ReadInt16LittleEndian(packet);
        Assert.Equal(packet.Length, packetSize);
    }

    [Fact]
    public void Encrypt_WithKeyword_EncryptsAndMatchesExpectedPacket()
    {
        var packet = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x02, 0x0D, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
        var expectedPacket = new byte[] { 0x0C, 0x00, 0x7E, 0xBF, 0x38, 0xFE, 0x0D, 0xFC, 0x66, 0xF1, 0x3C, 0xFD };

        var keywordGenerator = new Mock<IKeywordGenerator>();
        keywordGenerator
            .Setup(x => x.Generate())
            .Returns(0x7E);
        
        PacketCriptography.Encrypt(packet, keywordGenerator.Object);
        Assert.Equal(expectedPacket, packet);
    }
    
    [Fact]
    public void Decrypt_WithPacketSizeBelowMinimum_ReturnsFalse()
    {
        var packet = new byte[] { 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        var result = PacketCriptography.Decrypt(packet);
        Assert.False(result);
    }

    [Fact]
    public void Decrypt_WithEncryptedPacket_ReturnsTrueAndMatchesExpectedPacket()
    {
        var packet = new byte[] { 0x0C, 0x00, 0x7E, 0xBF, 0x38, 0xFE, 0x0D, 0xFC, 0x66, 0xF1, 0x3C, 0xFD };
        var expectedPacket = new byte[] { 0x0C, 0x00, 0x7E, 0xBF, 0x02, 0x0D, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };

        var result = PacketCriptography.Decrypt(packet);
        
        Assert.Equal(expectedPacket, packet);
        Assert.True(result);
    }

    [Fact]
    public void Decrypt_WithInvalidKeyword_ReturnsFalse()
    {
        var packet = new byte[] { 0x0C, 0x00, 0x5C, 0xBF, 0x38, 0xFE, 0x0D, 0xFC, 0x66, 0xF1, 0x3C, 0xFD };
        
        var result = PacketCriptography.Decrypt(packet);
        Assert.False(result);
    }
}