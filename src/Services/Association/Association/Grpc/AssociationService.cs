using Grpc.Core;
using Microsoft.Extensions.Logging;
using MiniUrl.Association.Domain.Model;
using MiniUrl.Association.Infrastructure;
using MiniUrl.Shared.Domain;
using System;
using System.Threading.Tasks;

namespace GrpcAssociation
{
    public class AssociationService : Association.AssociationBase
    {
        private readonly ILogger<AssociationService> _logger;
        private readonly AssociationContext _associationContext;
        private readonly IKeyRepository _keyRepository;
        private readonly IKeyConverter _keyConverter;

        public AssociationService(
            AssociationContext associationContext,
            IKeyRepository keysRepository,
            IKeyConverter keyConverter,
            ILogger<AssociationService> logger)
        {
            _logger = logger;
            _associationContext = associationContext;
            _keyRepository = keysRepository;
            _keyConverter = keyConverter;
        }

        public override async Task<UrlAssociationReply> AddUrl(UrlRequest request, ServerCallContext context)
        {
            var key = await GetAvailableKeyAsync();
            var address = request.Address;

            if (!address.StartsWith("http://") && !address.StartsWith("https://"))
                address = "http://" + address;

            await _associationContext.AddAsync<Address>(new(key.Id, address));
            await _associationContext.SaveChangesAsync();

            return new UrlAssociationReply
            {
                Key = _keyConverter.Encode(key.Id),
                Address = request.Address
            };
        }

        private async Task<Key> GetAvailableKeyAsync()
        {
            var keysCount = await _keyRepository.CountAvailableKeys();

            Random rand = new();
            int skipRowsCount = rand.Next(keysCount);

            var key = await _keyRepository.GetAvailableKeyAsync(skipRowsCount);

            return key;
        }
    }
}
