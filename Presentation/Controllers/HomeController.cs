using _NET_MinimalAPI.Domain.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace _NET_MinimalAPI.Presentation.Controllers
{
    [Route("/")]
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
