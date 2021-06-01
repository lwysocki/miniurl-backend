using Microsoft.EntityFrameworkCore;
using MiniUrl.KeyManager.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Infrastructure.Repositories
{
    public class KeysManagerRepository : IKeysManagerRepository
    {
        private readonly KeysManagerContext _context;

        public KeysManagerRepository(KeysManagerContext context)
        {
            _context = context;
        }

        public async Task<int> CountAvailableKeys()
        {
            int keysCount = await _context.Keys.Where(key => key.State == KeyState.New).CountAsync();

            return keysCount;
        }

        public async Task<Key> GetAvailableKeyAsync(int skipRowsCount)
        {
            var key = await _context.Keys.Where(key => key.State == KeyState.New).OrderBy(key => key.Id).Skip(skipRowsCount).FirstOrDefaultAsync();
            key.State = KeyState.Reserved;

            _context.Keys.Update(key);
            await _context.SaveChangesAsync();

            return key;
        }
    }
}
