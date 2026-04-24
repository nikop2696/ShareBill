using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShareBill.Services;
using Asp.Versioning;

namespace ShareBill.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class HealthController : Controller
    {

        private readonly Services.HealthService _healthService;

        public HealthController(
            Services.HealthService services
            )
        {
            _healthService = services;
        }


        [HttpGet]
        public async Task<IActionResult> HealthCheck()
        {
            var isHealthy = await _healthService.CanReachDatabase();

            if (!isHealthy)
            {
                return StatusCode(500);
            }

            return Ok(new { status = "ok" });
        }
    }
}
