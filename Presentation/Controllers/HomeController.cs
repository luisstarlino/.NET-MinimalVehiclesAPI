using _NET_MinimalAPI.Domain.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace _NET_MinimalAPI.Presentation.Controllers
{
    [Route("/")] //[Route("[Controller]")]
    [ApiController]
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Home());
        }
    }
}
