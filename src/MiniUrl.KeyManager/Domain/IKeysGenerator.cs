using System.Collections.Generic;

namespace MiniUrl.KeyManager.Domain
{
    public interface IKeysGenerator
    {
        long Iteration { get; }

        IList<long> Generate();
    }
}