using ShareBill.Modules.Users.Domain.Request;
using ShareBill.Shared.Models;

namespace ShareBill.Modules.Users.Application
{
    public interface ISignUpUserService
    {
        Task<OperationResult<SignUpResponse>> RegisterUserAsync(UserRequests.UserSignUp request);
    }
}
