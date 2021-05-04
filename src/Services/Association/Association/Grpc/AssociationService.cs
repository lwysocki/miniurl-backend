using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcKeysManager;
using MiniUrl.Association.Infrastructure;
using MiniUrl.Association.Domain.Model;
using MiniUrl.Shared.Domain;

namespace GrpcAssociation
{
    public class AssociationService : Association.AssociationBase
    {
        private readonly ILogger<AssociationService> _logger;
        private readonly KeysManager.KeysManagerClient _keysManagerClient;
        private readonly AssociationContext _associationContext;
        private readonly IKeyConverter _keyConverter;

        public AssociationService(
            KeysManager.KeysManagerClient keysManagerClient,
            AssociationContext associationContext,
            IKeyConverter keyConverter,
            ILogger<AssociationService> logger)
        {
            _logger = logger;
            _keysManagerClient = keysManagerClient;
            _associationContext = associationContext;
            _keyConverter = keyConverter;
        }

        public override async Task<UrlAssociationReply> AddUrl(UrlRequest request, ServerCallContext context)
        {
            var reply = await _keysManagerClient.GetAvailableKeyIdAsync(new KeyIdRequest());

            await _associationContext.AddAsync<Address>(new (reply.Id, request.Address));
            await _associationContext.SaveChangesAsync();

            return new UrlAssociationReply
            {
                Key = _keyConverter.Encode(reply.Id),
                Address = request.Address
            };
        }
    }
}
