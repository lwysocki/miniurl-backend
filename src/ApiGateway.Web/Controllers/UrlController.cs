using ApiGateway.Web.Models;
using ApiGateway.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;
        private readonly IAssociationService _associationService;

        public UrlController(IUrlService urlService, IAssociationService associationService)
        {
            _urlService = urlService;
            _associationService = associationService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UrlAssociationData>> GetUrlAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Valid id is required");
            }

            var urlData = await _urlService.GetByIdAsync(id);

            if (urlData == null)
            {
                return BadRequest($"Url not found for {id}");
            }

            return urlData;
        }

        [HttpPost]
        public async Task<ActionResult<UrlAssociationData>> AddUrlRequestAsync([FromBody] UrlRequest url)
        {
            if (string.IsNullOrWhiteSpace(url.Address))
            {
                return BadRequest("Valid url is required");
            }

            return await _associationService.AddUrlAsync(url);
        }
    }
}
