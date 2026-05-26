using Polly;
using Polly.Retry;
using ShareBill.Modules.Users.Application;
using ShareBill.Modules.Users.Domain;
using ShareBill.Modules.Users.Domain.Request;
using ShareBill.Shared.Errors;
using ShareBill.Shared.Errors.AuthErrors;
using ShareBill.Shared.Infrastructure.Policies;
using ShareBill.Shared.Models;
using Supabase;
using Supabase.Gotrue.Exceptions;

namespace ShareBill.Modules.Users.Infrastructure
{
    public class SignUpUserService : ISignUpUserService
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

        public async Task<OperationResult<SignUpResponse>> RegisterUserAsync(UserRequests.UserSignUp request)
        {
            try
            {
                _logger.LogInformation("Attempting to sign up user with email: {Email}", request.Email);
                _logger.LogInformation("Validating Username");

                var isAvaiable = await IsUsernameAvailable(request.UserName);

                if (!isAvaiable)
                { 
                    _logger.LogWarning("Username: {UserName} is already taken.", request.UserName);
                    return OperationResult<SignUpResponse>.Fail(AuthErrors.UserAlreadyInUse);
                }

                var signUpResponse = await CreateAuthUserAsync(request.Email, request.Password);

                if (!signUpResponse.Success)
                {
                    _logger.LogError(
                        "Failed to sign up user with email: {Email}. ErrorCode: {ErrorCode} - {Message}",
                        request.Email, signUpResponse.ErrorCode, signUpResponse.Message);

                    return OperationResult<SignUpResponse>.Fail(AuthErrorResolver.Resolve(signUpResponse.ErrorCode));
                }

                var userId = signUpResponse.Data!.UserID;

                _logger.LogInformation("User signed up successfully with email: {Email}", request.Email);
                _logger.LogInformation("Attempting to create users value for Username: {UserName}", request.UserName);

                var usernameUpdateResponse = await InsertUserName(userId, request.UserName);

                if (!usernameUpdateResponse.Success)
                {
                    _logger.LogError(
                        "Failed to create user profile for user with email: {Email}. ErrorCode: {ErrorCode} - {Message}",
                        request.Email, usernameUpdateResponse.ErrorCode, usernameUpdateResponse.Message);

                    return OperationResult<SignUpResponse>.Fail(AuthErrorResolver.Resolve(usernameUpdateResponse.ErrorCode));
                }
                return OperationResult<SignUpResponse>.Ok(new SignUpResponse(), "User signed up and profile created successfully.");

            }
            catch (Exception ex)
            {
                var (level,paylod) = ex.ToLog();
                _logger.Log(level,ex, "Exception occurred while signing up user with email: {Email}. {@Payload}", request.Email, paylod);
                

                return OperationResult<SignUpResponse>.Fail(ex);
            }


        }

        private async Task<bool> IsUsernameAvailable(string username)
        {

            var response = await _supaBaseService
                                .From<Profile>()
                                .Where(p => p.UserName == username)
                                .Get();
        
            return !(response.Models.Count > 0);

            
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
                        return OperationResult<AuthResponse>.Fail(AuthErrors.SupabaseInvalidSignUpResponse);
                    }

                    
                    _logger.LogInformation("User signed up successfully with email: {Email}", email);

                    return OperationResult<AuthResponse>.Ok(new AuthResponse { UserID = Guid.Parse(authSupaBaseResponse.User.Id)}, "User signUp successfully");
                });
            }
            catch (GotrueException gotrueEx)
            {
                var (level, payload) = gotrueEx.ToLog();
                var error = gotrueEx.ExtractErrorCode();
                _logger.Log(level, gotrueEx, "Exception occurred while signing up user with email: {Email}. {@Payload}", email, payload);
               
                return OperationResult<AuthResponse>.Fail(error);
            }
            catch (Exception ex)
            {
                var(level, payload) = ex.ToLog();
                _logger.Log(level, ex, "Exception occurred while signing up user with email: {Email}. {@Payload}", email, payload);
                return OperationResult<AuthResponse>.Fail(ex);
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
                    .Set(p => p.UserName!, userName)
                    .Update();

                    if (userUpdatetResponse == null)
                    {
                        _logger.LogError("Failed to update username for user with ID: {UserId}", userId);
                        return OperationResult<UserResponse>.Fail(AuthErrors.SignUpProfileUpdateFailed);
                    }
                    if(userUpdatetResponse.Models.Count == 0)
                    {
                        _logger.LogError("No profile found for user with ID: {UserId} when updating username.", userId);
                        return OperationResult<UserResponse>.Fail(AuthErrors.SignUpUsernameNotFound);
                    }
                    if (userUpdatetResponse.Models.Count > 1)
                    {
                        _logger.LogWarning("Multiple profiles found for user with ID: {UserId} when updating username. This should not happen.", userId);
                        return OperationResult<UserResponse>.Fail(AuthErrors.MultipleUser);
                    }

                    return OperationResult<UserResponse>.Ok(new UserResponse { UserID = userId, UserName = userName }, "Username updated successfully.");
                });
            }

            catch (Exception ex)
            {
                var(level, payload) = ex.ToLog();
                _logger.Log(level, ex, "Exception occurred while updating username for user with ID: {UserId}. {@Payload}", userId, payload);

                return OperationResult<UserResponse>.Fail(ex);
            }


        }


    }
}
