using ShareBill.Errors.AuthErrors;

namespace ShareBill.Errors.ConnectionErrors
{
    public class ConnectionErrors
    {
        public static AppErrorInfo UnReachableServer => new()
        {
            Code = "UnreachableServer",
            Type = ErrorType.Network,
            Severity = ErrorSeverity.High,
            IsRetryable = true,
            Description = "The server is currently unreachable. Please check your network connection and try again."
        };
    }
}
