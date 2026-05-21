using System.Text.Json.Serialization;

namespace ShareBill.Modules.Users.Domain.Entities
{
    public class UsersResponse
    {
        //public required string Id { get; set; }
        //public string? Username { get; set; }

        public sealed class LoginResponse
        {
            public required string AccessToken { get; set; } = string.Empty;
            public required UserValue UserInfo { get; set; }
            public required string RefreshToken {  get; set; } = string.Empty;
            public required string TokenType { get; set; } = string.Empty;
            public required int ExpiresIn { get; set; }
        }
        public sealed class UserValue
        {
            public string? Username { get; set; } = string.Empty;
            public bool IsActive { get; set; } = false;
            public bool IsCompleted { get; set; } = false;

        }

        internal sealed class SupabaseLoginResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; init; } = string.Empty;

            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; init; } = string.Empty;

            [JsonPropertyName("token_type")]
            public string TokenType { get; init; } = string.Empty;

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; init; }
            [JsonPropertyName("user")]
            public required SupabaseUser User { get; init; }
        }
        internal sealed class SupabaseUser 
        {
            [JsonPropertyName("id")]
            public required Guid Id {  get; set; }
        }

        internal sealed class UserTableResponse
        {
            public string? username { get; set; }
            public bool is_active { get; set; }
            public bool profile_completed { get; set; }
        }

    }

}
