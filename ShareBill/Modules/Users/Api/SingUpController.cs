using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShareBill.Modules.Users.Application;
using ShareBill.Modules.Users.Domain.Request;
using ShareBill.Shared.Models;

namespace ShareBill.Modules.Users.Api
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SignUpController : ControllerBase
    {
        private readonly SignUpUserService _signUpUserService;
        private readonly IValidator<UserRequests.UserSignUp> _validator;
        private readonly ILogger<SignUpController> _logger;

        public SignUpController(SignUpUserService signUpUserService, IValidator<UserRequests.UserSignUp> validator, ILogger<SignUpController> logger)
        {
            _signUpUserService = signUpUserService;
            _validator = validator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserRequests.UserSignUp request)
        {
            _logger.LogInformation("Received sign-up request for email: {Email} with username: {UserName}", request.Email, request.UserName);
            _logger.LogDebug("Validating sign-up request for email: {Email}", request.Email);
            var validationResult = await _validator.ValidateAsync(request);
            if(!validationResult.IsValid) 
            {
                var errors = string.Join("||", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for sign-up request with email: {Email}. Errors: {Errors}", request.Email, errors);
                return BadRequest(new OperationResult<SignUpResponse> { Success = false, Message = errors });
            }

            _logger.LogInformation("Validation succeeded for sign-up request with email: {Email}. Proceeding to register user.", request.Email);

            var result = await _signUpUserService.RegisterUserAsync(request);
            
            if (!result.Success)
            {
                _logger.LogError("User registration failed for email: {Email}. Error: {ErrorMessage}", request.Email, result.Message);
                return BadRequest(new OperationResult<SignUpResponse> { Success = false, Message = result.Message });
            }

            _logger.LogInformation("User registration succeeded for email: {Email}.", request.Email);
            return Ok(new OperationResult<SignUpResponse> { Success = true, Message = result.Message });
        }
    }
}
