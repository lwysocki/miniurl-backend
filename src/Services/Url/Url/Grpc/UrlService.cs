using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniUrl.Shared.Domain;
using MiniUrl.Url.Infrastructure;
using System.Threading.Tasks;

namespace GrpcUrl
{
    public class UrlService : Url.UrlBase
    {
        private readonly ILogger<UrlService> _logger;
        private readonly UrlContext _context;
        private readonly IKeyConverter _keyConverter;

        public UrlService(
            UrlContext context,
            IKeyConverter keyConverter,
            ILogger<UrlService> logger)
        {
            _context = context;
            _keyConverter = keyConverter;
            _logger = logger;
        }

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
