using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Utility
{
    public static class JsonDocumentExtensions
    {
        public static string ToJsonString(this JsonDocument jsonDocument)
        {
            var stream = new MemoryStream();
            Utf8JsonWriter writer = new(stream);
            jsonDocument.WriteTo(writer);
            writer.Flush();

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
