using ApiGateway.Web.Models;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<UrlAssociationData>> GetUrl(string id)
        {
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<UrlAssociationData>> StoreUrlRequest([FromBody] UrlRequest url)
        {
            return BadRequest();
        }
    }
}
