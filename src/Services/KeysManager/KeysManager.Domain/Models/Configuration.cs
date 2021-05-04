using System.Text.Json;

namespace MiniUrl.KeyManager.Domain.Models
{
    public abstract class Configuration
    {
        public string Key { get; set; }
        public JsonDocument Value { get; set; }
    }
}
