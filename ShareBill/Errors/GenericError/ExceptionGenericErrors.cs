using ShareBill.Errors.AuthErrors;

namespace ShareBill.Errors.GenericError
{
    public class ExceptionGenericErrors
    {
        public static readonly Dictionary<string, AppErrorInfo> ErrorMap = 
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["System.IO.IOException"] = new()
                {
                    Code = "IOException",
                    Type = ErrorType.IO,
                    Description = "An I/O error occurred while performing the operation.",
                    IsRetryable = true,
                    Severity = ErrorSeverity.High
                },
                ["System.UnauthorizedAccessException"] = new()
                {
                    Code = "UnauthorizedAccessException",
                    Type = ErrorType.Security,
                    Description = "The caller does not have the required permission.",
                    IsRetryable = false,
                    Severity = ErrorSeverity.High
                },
                ["System.NullReferenceException"] = new()
                {
                    Code = "NullReferenceException",
                    Type = ErrorType.Server,
                    Description = "An attempt was made to dereference a null object reference.",
                    IsRetryable = false,
                    Severity = ErrorSeverity.High
                },
                ["System.TimeoutException"] = new()
                {
                    Code = "TimeoutException",
                    Type = ErrorType.Timeout,
                    Description = "The operation has timed out.",
                    IsRetryable = true,
                    Severity = ErrorSeverity.High
                },
            };
    }
}
