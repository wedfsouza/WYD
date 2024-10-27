namespace WYD.Network.Criptography;

public sealed class KeywordGenerator : IKeywordGenerator
{
    public byte Generate() => (byte)Random.Shared.Next(256);
}