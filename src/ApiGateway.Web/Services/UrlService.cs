using ApiGateway.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Web.Services
{
    public class UrlService : IUrlService
    {
        public Task<UrlAssociationData> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
