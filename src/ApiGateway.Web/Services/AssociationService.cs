using ApiGateway.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcAssociation;

namespace ApiGateway.Web.Services
{
    public class AssociationService : IAssociationService
    {
        private readonly Association.AssociationClient _associationClient;

        public AssociationService(Association.AssociationClient associationClient)
        {
            _associationClient = associationClient;
        }

        public async Task<UrlAssociationData> AddUrlAsync(Models.UrlRequest url)
        {
            var response = await _associationClient.AddUrlAsync(new GrpcAssociation.UrlRequest() { Address = url.Address });

            var data = new UrlAssociationData()
            {
                Key = response.Key,
                Address = response.Address
            };

            return data;
        }
    }
}
