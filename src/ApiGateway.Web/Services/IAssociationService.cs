using ApiGateway.Web.Models;
using System.Threading.Tasks;

namespace ApiGateway.Web.Services
{
    public interface IAssociationService
    {
        Task<UrlAssociationData> AddUrlAsync(UrlRequest url);
    }
}