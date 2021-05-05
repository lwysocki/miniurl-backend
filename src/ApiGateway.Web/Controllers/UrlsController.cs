using Microsoft.AspNetCore.Mvc;
using MiniUrl.ApiGateway.Web.Models;
using MiniUrl.ApiGateway.Web.Services;
using System.Threading.Tasks;

namespace MiniUrl.ApiGateway.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    public class UrlsController : ControllerBase
    {
        private readonly IUrlService _urlService;
        private readonly IAssociationService _associationService;

        public UrlsController(IUrlService urlService, IAssociationService associationService)
        {
            _urlService = urlService;
            _associationService = associationService;
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<UrlAssociationData>> GetUrlAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest("Valid key is required");
            }

            var urlData = await _urlService.GetByKeyAsync(key);

            if (urlData == null)
            {
                return BadRequest($"Url not found for {key}");
            }

            return Redirect(urlData.Address);
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
