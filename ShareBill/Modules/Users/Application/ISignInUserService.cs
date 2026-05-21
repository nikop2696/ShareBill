using ShareBill.Modules.Users.Domain.Entities;
using ShareBill.Modules.Users.Domain.Request;
using ShareBill.Shared.Models;

namespace ShareBill.Modules.Users.Application
{
    public interface ISignInUserService
    {
        Task<OperationResult<UsersResponse.LoginResponse>> LoginAsync(LoginRequest request);
    }
}
