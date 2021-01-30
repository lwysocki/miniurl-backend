using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain.Models
{
    public enum KeyState: byte
    {
        New = 1,
        Reserved = 2,
        Used = 3
    }
}
