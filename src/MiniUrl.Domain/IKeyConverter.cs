using System.Collections.Generic;

namespace MiniUrl.Domain
{
    public interface IKeyConverter
    {
        int AlphabetSize { get; }
        string Encode(long number);
        long Decode(string key);
    }
}