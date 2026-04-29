using Microsoft.IdentityModel.Tokens;

namespace ShareBill.Errors.AuthErrors
{
    public static class AuthErrorResolver
    {
        public static AppErrorInfo Resolve(string errorCode)
        {
            if (string.IsNullOrWhiteSpace(errorCode))
            {
                return Uknown();
            }
            if (SupabaseAuthErrors.ErrorMap.TryGetValue(errorCode, out var errorInfo))
            {
                return errorInfo;
            }
            // Return a default error info for unknown errors
            return Uknown();
        }
        // Return a default error info for unknown errors
        private static AuthAppErrorInfo Uknown() => new()
        {
            Code = "unknown_error",
            Description = "An unknown authentication error occurred.",
            Type = ErrorType.Unknown,
            IsRetryable = false,
            HttpStatusCode = 0
        };
        public static AuthAppErrorInfo FromException(Exception ex)
        {
            return new ()
            {
                Code = "UnhandledException",
                Description = $"An unhandled exception occurred. Ex: {ex.GetFullExceptionMessage()}",
                Type = ErrorType.Unknown,
                IsRetryable = false,
                HttpStatusCode = 0
            };


        }
    }
}
