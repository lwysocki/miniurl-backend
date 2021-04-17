using System.Collections.Generic;

namespace MiniUrl.KeyManager.Domain
{
    public interface IKeyConverter
    {
        int AlphabetSize { get; }
        string Encode(long number);
        long Decode(string key);
    }
}