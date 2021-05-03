using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain.Models
{
    public class KeyGeneratorConfiguration : Configuration
    {
        public KeyGeneratorConfiguration()
        {
            Key = "KeyManager." + this.GetType().Name;
        }
    }
}
