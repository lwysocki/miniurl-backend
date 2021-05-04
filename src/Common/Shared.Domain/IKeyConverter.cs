namespace MiniUrl.Shared.Domain
{
    public interface IKeyConverter
    {
        int AlphabetSize { get; }
        string Encode(long number);
        long Decode(string key);
    }
}