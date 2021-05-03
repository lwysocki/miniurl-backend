using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace MiniUrl.KeyManager.Domain.Models
{
    public abstract class Configuration
    {
        public string Key { get; set; }
        public JsonDocument Value { get; set; }
    }
}
