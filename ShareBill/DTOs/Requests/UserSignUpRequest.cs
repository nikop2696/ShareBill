using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShareBill.DTOs.Requests
{
    public class UserSignUpRequest
    {
        [Required, NotNull]
        public required string Email { get; set; }
        [Required, NotNull]
        public required string Password { get; set; }
        [Required, NotNull]
        public required string UserName { get; set; }
    }
}
