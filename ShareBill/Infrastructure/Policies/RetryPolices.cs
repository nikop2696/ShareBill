using Polly;
using Polly.Retry;
using Npgsql;

namespace ShareBill.Infrastructure.Policies

{
    public static class RetryPolices
    {
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
    }
}
