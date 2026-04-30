using ShareBill.Errors.AuthErrors;

namespace ShareBill.Errors.GenericError
{
    public class GenericErrorResolver
    {
        public static AppErrorInfo Resolve(Exception ex)
        {
            if (ex == null)
            {
                return Uknown();
            }

            var key = ex.GetType().Name;
            if (key != null && ExceptionGenericErrors.ErrorMap.TryGetValue(key, out var errorInfo))
            {
                return new AppErrorInfo
                {
                    Code = errorInfo.Code,
                    Description = errorInfo.Description,
                    Type = errorInfo.Type,
                    IsRetryable = errorInfo.IsRetryable,
                    Severity = errorInfo.Severity,
                };
            }
            return new()
            {
                Code = key,
                Description = ex.Message,
                Type = ErrorType.Server,
                IsRetryable = false,
                Severity = ErrorSeverity.Medium,
            };
        }
        // Return a default error info for unknown errors
        private static AppErrorInfo Uknown() => new()
        {
            Code = "unknown_error",
            Description = "An unknown error occurred.",
            Type = ErrorType.Unknown,
            IsRetryable = false,
            Severity = ErrorSeverity.Medium,

        };
    }
}
