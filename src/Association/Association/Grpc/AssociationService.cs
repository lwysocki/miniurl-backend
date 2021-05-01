using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcAssociation
{
    public class AssociationService : Association.AssociationBase
    {
        private readonly ILogger<AssociationService> _logger;
        public AssociationService(ILogger<AssociationService> logger)
        {
            _logger = logger;
        }

        public override async Task<UrlAssociationReply> AddUrl(UrlRequest request, ServerCallContext context)
        {
            var response = await Task.Run(() =>
            {
                return new UrlAssociationReply
                {
                    Key = string.Empty,
                    Address = request.Address
                };
            });

            return response;
        }
    }
}
