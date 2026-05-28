using Dapper;
using Newtonsoft.Json;
using ShareBill.Modules.Users.Application;
using ShareBill.Modules.Users.Domain.Entities;
using ShareBill.Modules.Users.Domain.Request;
using ShareBill.Shared.Errors;
using ShareBill.Shared.Errors.AuthErrors;
using ShareBill.Shared.Infrastructure.Database;
using ShareBill.Shared.Infrastructure.Policies;
using ShareBill.Shared.Models;
using Supabase;

namespace ShareBill.Modules.Users.Infrastructure
{
    public class UserSignInService :ISignInUserService
    {
        private readonly Client _supaBaseService;
        IDbConnectionFactory _dbFactory;
        private readonly ILogger<OperationResult<UsersResponse.LoginResponse>> _logger;
        private readonly IRetryPolicies _retryPolicies;

        public UserSignInService(Client supaBaseService, ILogger<OperationResult<UsersResponse.LoginResponse>> logger, IRetryPolicies retryPolicies, IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
            _supaBaseService = supaBaseService;
            _logger = logger;
            _retryPolicies = retryPolicies;
        }

        public async Task<OperationResult<UsersResponse.LoginResponse>> SignInUserAsync(LoginRequest.Login request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    _logger.LogWarning("Login attempt with missing email or password");
                    return OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                }

                _logger.LogInformation("Attempting to sign in user with email: {Email}", request.Email);

                var authSupaBaseResponse = await _supaBaseService.Auth.SignIn(email: request.Email, password: request.Password);

                if (!IsValidSupabaseResponse(authSupaBaseResponse, out var loginResponse))
                {
                    return loginResponse!;
                }

                if(!Guid.TryParse(authSupaBaseResponse?.User?.Id, out Guid Id))
                {
                    _logger.LogWarning("Failed to parse user ID from Supabase response for email: {Email}. User ID value: {UserId}", request.Email, authSupaBaseResponse?.User?.Id);
                    return OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                }

                var usernameInfoResult = await GetUsernameInfo(Id);

                if (usernameInfoResult == null || !usernameInfoResult.Success || usernameInfoResult.Data == null)
                {
                    _logger.LogWarning("Impossible to Login - username info not found");
                    return OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignUpUsernameNotFound);
                }

                UsersResponse.LoginResponse user = new UsersResponse.LoginResponse
                {
                    Id = Id,
                    Email = authSupaBaseResponse.User.Email!,
                    AccessToken = authSupaBaseResponse.AccessToken!,
                    ExpiresIn = authSupaBaseResponse.ExpiresIn,
                    TokenType = authSupaBaseResponse.TokenType!,
                    RefreshToken = authSupaBaseResponse.RefreshToken!,
                    UserInfo = usernameInfoResult.Data!


                };


                return OperationResult<UsersResponse.LoginResponse>.Ok(user);


            }
            catch (Exception ex)
            {
                var (level, paylod) = ex.ToLog();
                _logger.Log(level, ex, "Exception occurred while log in user with email: {Email}. {@Payload}", request.Email, paylod);

                return OperationResult<UsersResponse.LoginResponse>.Fail(ex);
            }
        }


        private async Task<OperationResult<UsersResponse.UserValue>> GetUsernameInfo(Guid userId)
        {
            await using var connection = _dbFactory.CreateConnection();

            await connection.OpenAsync();

            var result = connection.QuerySingleOrDefault<UsersResponse.UserValue>(
                UsernameSql,
                new { UserId = userId });


            if(result == null)
            {
                return OperationResult<UsersResponse.UserValue>.Fail(AuthErrors.SignUpUsernameNotFound);
            }

            return OperationResult<UsersResponse.UserValue>.Ok(result);


        }

        private bool IsValidSupabaseResponse(Supabase.Gotrue.Session? authSupaBaseResponse, out OperationResult<UsersResponse.LoginResponse>? loginResponse)
        {
            loginResponse = null;
            if (authSupaBaseResponse == null)
            {
                _logger.LogWarning("Impossible to Login, the Sign In response return empty");
                loginResponse = OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                return false;
            }
            if (authSupaBaseResponse?.User == null)
            {
                _logger.LogWarning("Impossible to Login, the Sign In response return empty user");
                loginResponse = OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                return false;
            }
            if (string.IsNullOrWhiteSpace(authSupaBaseResponse?.User?.Id))
            {
                _logger.LogWarning("Impossible to Login, the Sign In response return user with empty id");
                loginResponse = OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                return false;
            }
            if (string.IsNullOrWhiteSpace(authSupaBaseResponse?.AccessToken))
            {
                _logger.LogWarning("Impossible to Login, the Sign In response return empty access token");
                loginResponse = OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                return false;
            }
            if(string.IsNullOrWhiteSpace(authSupaBaseResponse?.RefreshToken))
            {
                _logger.LogWarning("Impossible to Login, the Sign In response return empty refresh token");
                loginResponse = OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                return false;
            }
            if(string.IsNullOrWhiteSpace(authSupaBaseResponse?.TokenType))
            {
                _logger.LogWarning("Impossible to Login, the Sign In response return empty token type");
                loginResponse = OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                return false;
            }
            if(authSupaBaseResponse.ExpiresIn <= 0)
            {
                _logger.LogWarning("Impossible to Login, the Sign In response return invalid expires in value: {ExpiresIn}", authSupaBaseResponse.ExpiresIn);
                loginResponse = OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                return false;
            }

            return true;
        }

        private const string UsernameSql = @"SELECT username, is_active, profile_completed
                                                FROM public.users
                                                WHERE id = @UserId";

    }
}
