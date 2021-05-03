using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain.Models
{
    public class KeyServiceConfiguration : Configuration
    {
        public KeyServiceConfiguration()
        {
            Key = "KeyManager." + this.GetType().Name;
        }
    }
}
