using Microsoft.AspNetCore.Mvc;

namespace MiniUrl.ApiGateway.Web.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
