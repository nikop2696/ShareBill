using Polly;
using ShareBill.DTOs.Responses;

namespace ShareBill.Infrastructure.Policies
{
    public interface IRetryPolicies
    {
        IAsyncPolicy DBRetryPolicy { get; }
        IAsyncPolicy<UserResponse> SignUpRetryPolicy { get; }

    }
}
