using Grpc.Core;
using Microsoft.Extensions.Logging;
using MiniUrl.Association.Domain.Model;
using MiniUrl.Association.Infrastructure;
using MiniUrl.Association.Services;
using MiniUrl.Shared.Domain;
using System;
using System.Threading.Tasks;

namespace GrpcAssociation
{
    public class AssociationService(
        AssociationContext associationContext,
        IIdGenerator idGenerator,
        IKeyConverter keyConverter,
        ILogger<AssociationService> logger) : Association.AssociationBase
    {
        private readonly ILogger<AssociationService> _logger = logger;
        private readonly AssociationContext _associationContext = associationContext;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IKeyConverter _keyConverter = keyConverter;

        public override async Task<UrlAssociationReply> AddUrl(UrlRequest request, ServerCallContext context)
        {
            var key = await _idGenerator.GenerateIdAsync();
            var address = request.Address;

            if (!address.StartsWith("http://") && !address.StartsWith("https://"))
                address = "http://" + address;

            await _associationContext.AddAsync<Address>(new(key, address));
            await _associationContext.SaveChangesAsync();

            var encodedKey = _keyConverter.Encode(key);

            return new UrlAssociationReply
            {
                Key = encodedKey,
                Address = request.Address
            };
        }
    }
}
