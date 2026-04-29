using Polly;
using ShareBill.DTOs.Responses;

namespace ShareBill.Infrastructure.Policies
{
    public interface IRetryPolicies
    {
        IAsyncPolicy SignUpRetryPolicy { get; }
        IAsyncPolicy DBRetryPolicy { get; }
        IAsyncPolicy<OperationResult<UserResponse>> UsernameRetryPolicy { get; }

    }
}
