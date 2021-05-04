using Microsoft.Extensions.Options;
using MiniUrl.KeyManager.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MiniUrl.KeyManager.Services
{
    public class KeysGeneratorService : IKeysGeneratorService
    {
        public class KeysGeneratorSettings
        {
            public const string Section = "KeysGenerator";

            public long Iteration { get; set; }
            public long Limit { get; set; }
            public long Step { get; set; }
        }

        public JsonDocument SettingsJson
        {
            get
            {
                return JsonDocument.Parse(JsonSerializer.Serialize(Settings));
            }

            set
            {
                Settings = JsonSerializer.Deserialize<KeysGeneratorSettings>(value.ToJsonString());
            }
        }

        public KeysGeneratorSettings Settings { get; private set; }

        public KeysGeneratorService(IOptions<KeysGeneratorSettings> settings)
        {
            Settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public IList<long> Generate()
        {
            IList<long> keys = new List<long>();
            long current = ++Settings.Iteration;

            while (current < Settings.Limit)
            {
                keys.Add(current);
                current += Settings.Step;
            }

            return keys;
        }
    }
}
