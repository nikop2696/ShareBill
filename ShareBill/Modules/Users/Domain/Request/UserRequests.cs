namespace ShareBill.Modules.Users.Domain.Request
{
    public class UserRequests
    {
        public class UpdateUsername
        {
            public required Guid id { get; set; }
            public required string username { get; set; }
        }

        public class UserSignUp
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required string UserName { get; set; }
        }
    }
}
