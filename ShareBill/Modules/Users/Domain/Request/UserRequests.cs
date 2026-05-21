namespace ShareBill.Modules.Users.Domain.Request
{
    public sealed class UserRequests
    {
        public sealed class UsernameRequest
        {
            public required Guid Id { get; set; }
            public required string Username { get; set; }
        }

        public sealed class UserSignUp
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required string UserName { get; set; }
        }
    }

    public sealed class LoginRequest
    {
        public sealed class Login 
        {
            public required string Email { get; set; } = string.Empty;
            public required string Password { get; set; } = string.Empty;
        }

    }
}
