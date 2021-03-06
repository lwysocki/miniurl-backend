﻿using MiniUrl.ApiGateway.Web.Models;
using System.Threading.Tasks;

namespace MiniUrl.ApiGateway.Web.Services
{
    public interface IAssociationService
    {
        Task<UrlAssociationData> AddUrlAsync(UrlRequest url);
    }
}