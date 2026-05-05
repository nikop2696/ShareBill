using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShareBill.DTOs.Requests
{
    public class UserSignUpRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string UserName { get; set; }
    }
}
