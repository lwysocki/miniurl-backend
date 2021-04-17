using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using MiniUrl.KeyManager.Utility;

namespace MiniUrl.KeyManager.Domain
{
    public class KeysGenerator : IKeysGenerator
    {
        public class KeysGeneratorConfiguration
        {
            public long Iteration { get; set; }
            public long Limit { get; set; }
            public long Step { get; set; }
        }

        public JsonDocument ConfigurationJson {
            get
            {
                return JsonDocument.Parse(JsonSerializer.Serialize(Configuration));
            }

            set
            {
                Configuration = JsonSerializer.Deserialize<KeysGeneratorConfiguration>(value.ToJsonString());
            }
        }

        public KeysGeneratorConfiguration Configuration { get; private set; }

        public KeysGenerator(string configuration) : this(JsonDocument.Parse(configuration))
        {
        }

        public KeysGenerator(JsonDocument configuration)
        {
            ConfigurationJson = configuration;
        }

        public IList<long> Generate()
        {
            IList<long> keys = new List<long>();
            long current = ++Configuration.Iteration;

            while (current < Configuration.Limit)
            {
                keys.Add(current);
                current += Configuration.Step;
            }

            return keys;
        }
    }
}
