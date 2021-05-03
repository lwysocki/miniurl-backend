using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain.Models
{
    public interface IKeysServiceRepository
    {
        Task<int> CountAvailableKeys();
        Task<long> GetAvailableKeyIdAsync(int availableKeysCount);
    }
}
