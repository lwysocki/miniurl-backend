using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcKeysManager;

namespace GrpcAssociation
{
    public class AssociationService : Association.AssociationBase
    {
        private readonly ILogger<AssociationService> _logger;
        private readonly KeysManager.KeysManagerClient _keysManagerClient;

        public AssociationService(ILogger<AssociationService> logger, KeysManager.KeysManagerClient keysManagerClient)
        {
            _logger = logger;
            _keysManagerClient = keysManagerClient;
        }

        public override async Task<UrlAssociationReply> AddUrl(UrlRequest request, ServerCallContext context)
        {
            var reply = await _keysManagerClient.GetAvailableKeyIdAsync(new KeyIdRequest());
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
