using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniUrl.Shared.Domain;
using MiniUrl.Url.Infrastructure;
using System.Threading.Tasks;

namespace GrpcUrl
{
    public class UrlService(
        UrlContext context,
        IKeyConverter keyConverter,
        ILogger<UrlService> logger) : Url.UrlBase
    {
        private readonly ILogger<UrlService> _logger = logger;
        private readonly UrlContext _context = context;
        private readonly IKeyConverter _keyConverter = keyConverter;

        public override async Task<UrlAssociationReply> GetUrlByKey(KeyRequest request, ServerCallContext context)
        {
            var id = _keyConverter.Decode(request.Key);
            var address = await _context.Addresses.SingleAsync(a => a.Id == id);

            return new UrlAssociationReply
            {
                Key = request.Key,
                Address = address.Url
            };
        }
    }
}
