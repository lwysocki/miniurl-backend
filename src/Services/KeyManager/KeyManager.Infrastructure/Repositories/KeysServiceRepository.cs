using MiniUrl.KeyManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MiniUrl.KeyManager.Infrastructure.Repositories
{
    public class KeysServiceRepository : IKeysServiceRepository
    {
        private readonly KeyManagerContext _context;

        public KeysServiceRepository(KeyManagerContext context)
        {
            _context = context;
        }

        public async Task<int> CountAvailableKeys()
        {
            int keysCount = await _context.Keys.Where(key => key.State == KeyState.New).CountAsync();

            return keysCount;
        }

        public async Task<long> GetAvailableKeyIdAsync(int availableKeysCount)
        {
            Random rand = new();
            int rowsCount = rand.Next(availableKeysCount);

            var key = await _context.Keys.Where(key => key.State == KeyState.New).OrderBy(key => key.Id).Skip(rowsCount).FirstOrDefaultAsync();
            key.State = KeyState.Reserved;

            _context.Keys.Update(key);
            await _context.SaveChangesAsync();

            return key.Id;
        }
    }
}
