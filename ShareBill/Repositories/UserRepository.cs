using Dapper;
using ShareBill.DTOs.Requests;
using ShareBill.DTOs.Responses.Operation;
using ShareBill.Infrastructure.Database;

namespace ShareBill.Repositories
{
    public class UserRepository : IUsersRepository
    {

        private readonly IDbConnectionFactory _dbFactory;

        public UserRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<OperationResult<UserResponse>> UpdateUsernameAsync(UpdateUsernameRequest request)
        {
            using var connection = _dbFactory.CreateConnection();

            var query = @"
                    UPDATE users
                    SET username = @username 
                    WHERE id = @id";

            var rowsAffected = await connection.ExecuteAsync(query, new
            {
                id = request.id,
                username = request.username
            });
            if (rowsAffected == 0)
            {
                return new OperationResult<UserResponse>
                {
                    Success = false,
                    Message = "User not found.",
                    ErrorCode = "USER_NOT_FOUND",
                    IsRetryable = false
                };
            }

            return new OperationResult<UserResponse>
            {
                Success = true,
                Data = new UserResponse
                {
                    UserID = request.id,
                    UserName = request.username
                },
                Message = "Username updated successfully."
            };
        }
    }
}
