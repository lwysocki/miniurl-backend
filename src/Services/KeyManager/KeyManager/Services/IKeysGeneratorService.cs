using System.Collections.Generic;
using System.Text.Json;

namespace MiniUrl.KeyManager.Services
{
    public interface IKeysGeneratorService
    {
        JsonDocument ConfigurationJson { get; set; }

        IList<long> Generate();
    }
}