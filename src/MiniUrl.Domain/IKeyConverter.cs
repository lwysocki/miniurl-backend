namespace MiniUrl.Domain
{
    public interface IKeyConverter
    {
        int AlphabetSize { get; }
        string Encode(long number);
    }
}