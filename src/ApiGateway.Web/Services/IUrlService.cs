using MiniUrl.ApiGateway.Web.Models;
using System.Threading.Tasks;

namespace MiniUrl.ApiGateway.Web.Services
{
    public interface IUrlService
    {
        Task<UrlAssociationData> GetByKeyAsync(string key);
    }
}