using System.Collections.Generic;
using System.Text.Json;

namespace MiniUrl.KeyManager.Services
{
    public interface IKeysGeneratorService
    {
        JsonDocument SettingsJson { get; set; }

        IList<long> Generate();
    }
}