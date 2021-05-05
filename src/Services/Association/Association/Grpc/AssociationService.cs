using Grpc.Core;
using GrpcKeysManager;
using Microsoft.Extensions.Logging;
using MiniUrl.Association.Domain.Model;
using MiniUrl.Association.Infrastructure;
using MiniUrl.Shared.Domain;
using System.Threading.Tasks;

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

            var address = request.Address;

            if (!address.StartsWith("http://") && !address.StartsWith("https://"))
                address = "http://" + address;

            await _associationContext.AddAsync<Address>(new(reply.Id, address));
            await _associationContext.SaveChangesAsync();

            return new UrlAssociationReply
            {
                Key = _keyConverter.Encode(reply.Id),
                Address = request.Address
            };
        }
    }
}
