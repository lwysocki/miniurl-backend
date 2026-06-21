using System.Threading.Tasks;

namespace MiniUrl.Association.Services;

public interface IIdGenerator
{
    Task<long> GenerateIdAsync();
}
