using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Services
{
    interface IKeysManagerService
    {
        Task<long> GetAvailableKeyIdAsync();
    }
}
