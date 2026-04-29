using Polly;
using ShareBill.DTOs.Responses;

namespace ShareBill.Infrastructure.Policies
{
    public interface IRetryPolicies
    {
        IAsyncPolicy GoTrueRetryPolicy { get; }
        IAsyncPolicy DBRetryPolicy { get; }
        IAsyncPolicy<OperationResult<UserResponse>> SignUpRetryPolicy { get; }

    }
}
