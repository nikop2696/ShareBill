using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShareBill.Modules.Users.Application;
using ShareBill.Modules.Users.Domain.Entities;
using ShareBill.Modules.Users.Domain.Request;
using ShareBill.Shared.Models;

namespace ShareBill.Modules.Users.Presentation
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SignInController : Controller
    {
        private readonly ISignInUserService _signInUserService;
        private readonly ILogger<SignInController> _logger;
        private readonly IValidator<LoginRequest.Login> _validator;

        public SignInController(ISignInUserService signInUserService, ILogger<SignInController> logger, IValidator<LoginRequest.Login> validator)
        {
            _signInUserService = signInUserService;
            _logger = logger;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest.Login request)
        {
            _logger.LogInformation("Received sign-in request for email: {Email}", request.Email);
            _logger.LogInformation("Validating sign-in request");

            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("||", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for sign-in request: {Errors}", errors);
                return BadRequest(new OperationResult<UsersResponse.LoginResponse> { Success = false, Message = errors });
            }

            _logger.LogInformation("Validation succeeded for sign-in request with email: {Email}. Proceeding to sign in user.", request.Email);

            var result = await _signInUserService.SignInUserAsync(request);

            if (!result.Success)
            {
                _logger.LogError("User sign-in failed for email: {Email}. Error: {ErrorMessage}", request.Email, result.Message);
                return BadRequest(new OperationResult<UsersResponse.LoginResponse> { Success = false, Message = result.Message });
            }

            _logger.LogInformation("User sign-in succeeded for email: {Email}.", request.Email);
            return Ok(new OperationResult<UsersResponse.LoginResponse> { Success = true, Message = result.Message, Data = result.Data });
        }
    }
}
