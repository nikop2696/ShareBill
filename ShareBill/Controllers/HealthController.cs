using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShareBill.Services;

namespace ShareBill.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
