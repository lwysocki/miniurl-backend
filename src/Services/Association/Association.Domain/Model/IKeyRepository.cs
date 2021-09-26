using System.Threading.Tasks;

namespace MiniUrl.Association.Domain.Model
{
    public interface IKeyRepository
    {
        Task<int> CountAvailableKeys();
        Task<Key> GetAvailableKeyAsync(int skipRowsCount);
    }
}
