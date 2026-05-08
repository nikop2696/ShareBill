using Polly;
using ShareBill.Shared.Models;

namespace ShareBill.Shared.Infrastructure.Policies
{
    public interface IRetryPolicies
    {
        IAsyncPolicy SignUpRetryPolicy { get; }
        IAsyncPolicy DBRetryPolicy { get; }
        IAsyncPolicy<OperationResult<UserResponse>> UsernameRetryPolicy { get; }

    }
}
