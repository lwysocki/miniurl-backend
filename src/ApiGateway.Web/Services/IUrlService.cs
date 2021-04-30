using ApiGateway.Web.Models;
using System.Threading.Tasks;

namespace ApiGateway.Web.Services
{
    public interface IUrlService
    {
        Task<UrlAssociationData> GetByIdAsync(string id);
    }
}