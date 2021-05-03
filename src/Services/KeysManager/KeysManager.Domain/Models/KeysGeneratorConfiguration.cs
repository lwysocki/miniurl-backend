using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain.Models
{
    public class KeysGeneratorConfiguration : Configuration
    {
        public KeysGeneratorConfiguration()
        {
            Key = "KeyManager." + this.GetType().Name;
        }
    }
}
