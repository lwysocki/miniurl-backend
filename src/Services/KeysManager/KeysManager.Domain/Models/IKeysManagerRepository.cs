using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain.Models
{
    public interface IKeysManagerRepository
    {
        Task<int> CountAvailableKeys();
        Task<Key> GetAvailableKeyAsync(int skipRowsCount);
    }
}
