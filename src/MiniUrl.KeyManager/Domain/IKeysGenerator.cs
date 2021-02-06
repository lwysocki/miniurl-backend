﻿using System.Collections.Generic;
using System.Text.Json;

namespace MiniUrl.KeyManager.Domain
{
    public interface IKeysGenerator
    {
        JsonDocument ConfigurationJson { get; }

        IList<long> Generate();
    }
}