using Npgsql;
using Polly;
using Polly.Retry;
using ShareBill.DTOs.Responses;
using ShareBill.Errors;
using ShareBill.Errors.AuthErrors;
using Supabase.Gotrue.Exceptions;

namespace ShareBill.Infrastructure.Policies

{
    public class RetryPolicesProvider : IRetryPolicies
    {
        private readonly ILogger _logger;

        public RetryPolicesProvider(ILogger<RetryPolicesProvider> logger)
        {
            _logger = logger;
        }
        public IAsyncPolicy GoTrueRetryPolicy => GetGoTrueRetryPolicy(_logger);
        public  IAsyncPolicy DBRetryPolicy => GetDBRetryPolicy();
        public  IAsyncPolicy<OperationResult<UserResponse>> SignUpRetryPolicy => GetSignUpRetryPolicy(_logger);
        
        public static IAsyncPolicy GetGoTrueRetryPolicy(ILogger logger)
        {
            return Policy
                .Handle<GotrueException>(ex => ex.ExtractErrorCode().IsRetryable)
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: attempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, attempt)) + //Exponential retry (if the db is busy, increment the span of duration)
                    TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)) //Jitter (Add a litter jitter to avaid bombarding the DB with request
                    );
        } 

        /// <summary>
        /// Policy that handle NpgsqlException and TimeoutException by retry 5 time in a power of 2 time span
        /// </summary>
        /// <returns></returns>
        public static AsyncRetryPolicy GetDBRetryPolicy()
        {
            return Policy
                .Handle<NpgsqlException>(ex => ex.IsTransient)
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: attempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, attempt)) + //Exponential retry (if the db is busy, increment the span of duration)
                    TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)) //Jitter (Add a litter jitter to avaid bombarding the DB with request
                    );
        }

        public static AsyncRetryPolicy<OperationResult<UserResponse>> GetSignUpRetryPolicy(ILogger logger) 
        {
            return Policy
                .Handle<InvalidOperationException>()
                .OrResult<OperationResult<UserResponse>>(r => !r.Success)
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: attempt =>
                        TimeSpan.FromMilliseconds(300 * attempt),
                    onRetry: (response, timespan, retryCount, context) =>
                    {
                        logger.LogWarning(
                            "Retrying to update username for user creation. Attempt {RetryCount}. Waiting {TimeSpan} before next retry.",
                            retryCount,
                            timespan.TotalMilliseconds);
                    });
                
        }
    }


}
