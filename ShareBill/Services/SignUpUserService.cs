using Polly;
using Polly.Retry;
using ShareBill.Domain.Entities;
using ShareBill.DTOs;
using ShareBill.DTOs.Responses;
using ShareBill.Infrastructure.Policies;
using Supabase;

namespace ShareBill.Services
{
    public class SignUpUserService
    {
        private readonly Client _supaBaseService;
        private readonly ILogger<SignUpUserService> _logger;
        private readonly IRetryPolicies _retryPolicies;

        public SignUpUserService(Client supaBaseService, ILogger<SignUpUserService> logger, IRetryPolicies retryPolicies)
        {
            _supaBaseService = supaBaseService;
            _logger = logger;
            _retryPolicies = retryPolicies;
        }
        public async Task<SignUpResponse> RegisterUserAsync(UserSignUpRequest request)
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

                var userId = signUpResponse.UserID;

                _logger.LogInformation("User signed up successfully with email: {Email}", request.Email);

                _logger.LogInformation("Attempting to create users value for Username: {UserName}", request.UserName);

                var usernameUpdateResponse = await InsertUserName(userId, request.UserName);

                if (usernameUpdateResponse == null || !usernameUpdateResponse.Success)
                {
                    _logger.LogError("Failed to create user profile for user with email: {Email}", request.Email);

                    throw new Exception("Failed to create user profile.");
                }
                return new SignUpResponse { Success = true, Message = "User signed up and profile created successfully." };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during user sign up process for email: {Email}", request.Email);
                return new SignUpResponse { Success = false, Message = $"Exception occurred during user sign up process. Error: {ex.Message}" };
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

                return !response.Models?.Any() ?? true;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private async Task<AuthResponse> CreateAuthUserAsync(string email, string password)
        {
            try
            {
                var authSupaBaseResponse = await _supaBaseService.Auth.SignUp(email, password);

                if (authSupaBaseResponse == null || authSupaBaseResponse.User == null || string.IsNullOrWhiteSpace(authSupaBaseResponse.User.Id))
                {
                    _logger.LogError("Failed to sign up user with email: {Email}", email);
                    return new AuthResponse { Success = false, Message = "Failed to sign up user." };
                }
                _logger.LogInformation("User signed up successfully with email: {Email}", email);

                return new AuthResponse { Success = true, Message = "User signed up successfully.", UserID = Guid.Parse(authSupaBaseResponse.User.Id) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while signing up user with email: {Email}", email);
                return new AuthResponse { Success = false, Message = "Exception occurred while signing up user." };
            }
        }


        private async Task<UserResponse> InsertUserName(Guid userId, string userName)
        {
            try
            {
                return await _retryPolicies.SignUpRetryPolicy.ExecuteAsync(async () =>
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

                    return new UserResponse { Success = true, Message = "Username updated successfully." };
                });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating username for user with ID: {UserId}", userId);

                return new UserResponse { Success = false, Message = $"Exception occurred while updating username. Error: {ex.Message}" };
            }


        }

    }
}
