using Polly;
using Polly.Retry;
using ShareBill.Domain.Entities;
using ShareBill.DTOs.Requests;
using ShareBill.DTOs.Responses;
using ShareBill.Errors.AuthErrors;
using ShareBill.Infrastructure.Policies;
using Supabase;
using Supabase.Gotrue.Exceptions;

namespace ShareBill.Services
{
    public class SignUpUserService
    {
        private readonly Client _supaBaseService;
        private readonly ILogger<OperationResult<SignUpUserService>> _logger;
        private readonly IRetryPolicies _retryPolicies;

        public SignUpUserService(Client supaBaseService, ILogger<OperationResult<SignUpUserService>> logger, IRetryPolicies retryPolicies)
        {
            _supaBaseService = supaBaseService;
            _logger = logger;
            _retryPolicies = retryPolicies;
        }
        public async Task<OperationResult<SignUpResponse>> RegisterUserAsync(UserSignUpRequest request)
        {
            try
            {

                _logger.LogInformation("Attempting to sign up user with email: {Email}", request.Email);

                _logger.LogInformation("Validating Username");

                var isAvaiable = await IsUsernameAvailable(request.UserName);

                if (!isAvaiable) 
                {
                    throw new ArgumentException("Username already in use");
                }

                var signUpResponse = await CreateAuthUserAsync(request.Email, request.Password);

                if (signUpResponse == null || !signUpResponse.Success)
                {
                    _logger.LogError("Failed to sign up user with email: {Email}", request.Email);

                    throw new Exception("Failed to sign up user.");
                }

                var userId = signUpResponse.Data.UserID;

                _logger.LogInformation("User signed up successfully with email: {Email}", request.Email);

                _logger.LogInformation("Attempting to create users value for Username: {UserName}", request.UserName);

                var usernameUpdateResponse = await InsertUserName(userId, request.UserName);

                if (usernameUpdateResponse == null || !usernameUpdateResponse.Success)
                {
                    _logger.LogError("Failed to create user profile for user with email: {Email}", request.Email);

                    throw new Exception("Failed to create user profile.");
                }
                return new OperationResult<SignUpResponse> { Success = true, Message = "User signed up and profile created successfully." };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during user sign up process for email: {Email}", request.Email);

                return new OperationResult<SignUpResponse> { Success = false, Message = $"Exception occurred during user sign up process. Error: {ex.Message}" };
            }


        }

        private async Task<bool> IsUsernameAvailable(string username)
        {
            try
            {
                var response = await _supaBaseService
                                    .From<Profile>()
                                    .Where(p => p.UserName == username)
                                    .Get();

                return !(response.Models.Count > 0);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private async Task<OperationResult<AuthResponse>> CreateAuthUserAsync(string email, string password)
        {
            try
            {
                return await _retryPolicies.SignUpRetryPolicy.ExecuteAsync(async () =>
                {
                    var authSupaBaseResponse = await _supaBaseService.Auth.SignUp(email, password);

                    if (authSupaBaseResponse == null || authSupaBaseResponse.User == null || string.IsNullOrWhiteSpace(authSupaBaseResponse.User.Id))
                    {
                        _logger.LogError("Failed to sign up user with email: {Email}", email);
                        return new OperationResult<AuthResponse> { Success = false, Message = "Failed to sign up user." };
                    }
                    _logger.LogInformation("User signed up successfully with email: {Email}", email);

                    return new OperationResult<AuthResponse> { Success = true, Message = "User signed up successfully.", Data = new AuthResponse { UserID = Guid.Parse(authSupaBaseResponse.User.Id) } };
                });
            }
            catch (GotrueException gotrueEx)
            {
                var error = gotrueEx.ExtractErrorCode();
                _logger.LogError(gotrueEx, "GotrueException occurred while signing up user with email: {Email}. Error Code: {ErrorCode}. Error Message : {ErrorMessage}", email, error.Code, error.Description);
                return new OperationResult<AuthResponse> { Success = false, Message = $"GotrueException occurred while signing up user. Error Code: {error.Code}, Error Message: {error.Description}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while signing up user with email: {Email}", email);
                return new OperationResult<AuthResponse> { Success = false, Message = "Exception occurred while signing up user." };
            }
        }


        private async Task<OperationResult<UserResponse>> InsertUserName(Guid userId, string userName)
        {
            try
            {
                return await _retryPolicies.UsernameRetryPolicy.ExecuteAsync(async () =>
                {

                    var userUpdatetResponse = await _supaBaseService
                    .From<Profile>()
                    .Where(p => p.Id == userId)
                    .Set(p => p.UserName, userName)
                    .Update();

                    if (userUpdatetResponse == null || userUpdatetResponse.Models.Count == 0)
                    {
                        throw new InvalidOperationException("Profile update failed. No records were updated.");
                    }

                    return new OperationResult<UserResponse> { Success = true, Message = "Username updated successfully." };
                });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating username for user with ID: {UserId}", userId);

                return new OperationResult<UserResponse> { Success = false, Message = $"Exception occurred while updating username. Error: {ex.Message}" };
            }


        }

    }
}
