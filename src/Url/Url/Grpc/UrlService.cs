using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcUrl
{
    public class UrlService : Url.UrlBase
    {
        private readonly ILogger<UrlService> _logger;

        public UrlService(ILogger<UrlService> logger)
        {
            _logger = logger;
        }

        public override async Task<UrlAssociationReply> GetUrlById(UrlRequest request, ServerCallContext context)
        {
            var response = await Task.Run(() =>
            {
                return new UrlAssociationReply
                {
                    Id = request.Id,
                    Address = string.Empty
                };
            });

            return response;
        }
    }
}
