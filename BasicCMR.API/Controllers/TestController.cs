using Microsoft.AspNetCore.Mvc;

namespace BasicCMR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { message = "Basic CMR API is alive ⚡" });
        }
    }
}
