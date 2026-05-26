using Dapper;
using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json;
using ShareBill.Modules.Users.Domain.Entities;
using ShareBill.Shared.Errors.AuthErrors;
using ShareBill.Shared.Infrastructure.Database;
using ShareBill.Shared.Infrastructure.Policies;
using ShareBill.Shared.Models;
using Supabase;
using System.Security.Cryptography;

namespace ShareBill.Modules.Users.Infrastructure
{
    public class UserSignInService
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

        public async Task<OperationResult<UsersResponse.LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                _logger.LogInformation(@"Try to logIn user with Email: {User}", request.Email);

                var authSupaBaseResponse = await _supaBaseService.Auth.SignIn(email: request.Email, password: request.Password);
                if (authSupaBaseResponse == null)
                {
                    _logger.LogWarning("Impossible to Login");
                    return OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
                }

                var json = authSupaBaseResponse.ToString() ?? "{}";
                var user = JsonConvert.DeserializeObject<UsersResponse.SupabaseLoginResponse>(json);

                if (user == null)
                {
                    _logger.LogWarning("Impossible to Login - deserialization failed");
                    return OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.DeserealizationFailed);
                }

                // Map to public response DTO. If OperationResult has a different success factory, adjust accordingly.
                Guid Id = user.User.Id;

                var usernameInfoResult = await GetUsernameInfo(Id);

                if (usernameInfoResult == null || !usernameInfoResult.Success || usernameInfoResult.Data == null)
                {
                    _logger.LogWarning("Impossible to Login - username info not found");
                    return OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignUpUsernameNotFound);
                }

                UsersResponse.LoginResponse loginResponse = new UsersResponse.LoginResponse()
                {
                    AccessToken = user.AccessToken,
                    RefreshToken = user.RefreshToken,
                    UserInfo = usernameInfoResult.Data,
                    ExpiresIn = user.ExpiresIn,
                    TokenType = user.TokenType,
                };

                return OperationResult<UsersResponse.LoginResponse>.Ok(loginResponse);




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login");
                return OperationResult<UsersResponse.LoginResponse>.Fail(AuthErrors.SignInFailed);
            }
        }


        private async Task<OperationResult<UsersResponse.UserValue>> GetUsernameInfo(Guid userId)
        {
            await using var connection = _dbFactory.CreateConnection();

            await connection.OpenAsync();

            var result = connection.QuerySingleOrDefault<UsersResponse.UserDbModel>(
                UsernameSql,
                new { UserId = userId });
            if(result == null)
            {
                return null;
            }

            UsersResponse.UserValue user = new UsersResponse.UserValue()
            {
                Username = result.username,
                IsActive = result.is_active,
                IsCompleted = result.profile_completed
            };


            return OperationResult<UsersResponse.UserValue>.Ok(user);


        }

        private const string UsernameSql = @"SELECT username, is_active, profile_completed
                                                FROM public.users
                                                WHERE id = @UserId";

    }
}
