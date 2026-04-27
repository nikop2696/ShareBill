using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ShareBill.DTOs.Responses;
using ShareBill.Services;

namespace ShareBill.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SignUpController : ControllerBase
    {
        private readonly SignUpUserService _signUpUserService;

        public SignUpController(SignUpUserService signUpUserService)
        {
            _signUpUserService = signUpUserService;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] DTOs.UserSignUpRequest request)
        {
            var result = await _signUpUserService.RegisterUserAsync(request);
            if (!result.Success)
            {
                return BadRequest(new BaseResponse { Success = false, Message = result.Message });
            }
            return Ok(new BaseResponse { Success = true, Message = result.Message });
        }
    }
}
