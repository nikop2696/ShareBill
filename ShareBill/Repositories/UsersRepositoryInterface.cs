using Npgsql.Replication.PgOutput.Messages;
using ShareBill.DTOs.Requests;
using ShareBill.DTOs.Responses.Operation;

namespace ShareBill.Repositories
{
    public interface IUsersRepository
    {
        Task<OperationResult<UserResponse>> UpdateUsernameAsync(UpdateUsernameRequest updateRequest);
    }

}
