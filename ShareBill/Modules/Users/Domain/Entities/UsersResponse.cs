using System.Text.Json.Serialization;

namespace ShareBill.Modules.Users.Domain.Entities
{
    public class UsersResponse
    {
        //public required string Id { get; set; }
        //public string? Username { get; set; }

        public sealed class LoginResponse
        {
            public required Guid Id { get; set; }
            public required string Email { get; set; } = string.Empty;
            public required string AccessToken { get; set; } = string.Empty;
            public required string RefreshToken {  get; set; } = string.Empty;
            public required string TokenType { get; set; } = string.Empty;
            public required long ExpiresIn { get; set; }
            public required UserValue UserInfo { get; set; }
        }
        public sealed class UserValue
        {
            [JsonPropertyName("username")]
            public string? Username { get; set; } = string.Empty;
            [JsonPropertyName("is_active")]
            public bool IsActive { get; set; } = false;
            [JsonPropertyName("profile_completed")]
            public bool IsCompleted { get; set; } = false;

        }

    }

}
