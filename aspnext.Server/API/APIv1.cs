using Microsoft.AspNetCore.Mvc;

namespace aspnext.Server.API;

[ApiController]
[Route("api/v1")]
public class APIv1 : Controller {

    [HttpGet, HttpGet("hello")]
    public IActionResult Index() {
        return Ok(new {
            status = "ok",
            message = "API v1 is running",
            timestamp = DateTime.UtcNow
        });
    }
}