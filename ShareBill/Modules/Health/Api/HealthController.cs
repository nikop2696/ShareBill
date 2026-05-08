using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ShareBill.Modules.Health.Application;

namespace ShareBill.Modules.Health.Api
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class HealthController : Controller
    {

        private readonly HealthService _healthService;

        public HealthController(
            HealthService services
            )
        {
            _healthService = services;
        }


        [HttpGet]
        public async Task<IActionResult> HealthCheck()
        {
            var isHealthy = await _healthService.CanReachDatabase();

            if (!isHealthy.Data)
            {
                return StatusCode(500);
            }

            return Ok(new { status = "ok" });
        }
    }
}
