using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain.Models
{
    public class KeyManagerConfiguration : Configuration
    {
        public KeyManagerConfiguration()
        {
            Key = this.GetType().Name;
        }
    }
}
