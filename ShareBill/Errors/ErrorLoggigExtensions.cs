using ShareBill.Errors.GenericError;
using System.Runtime.CompilerServices;

namespace ShareBill.Errors
{
    public static class ErrorLoggigExtensions
    {
        public static (LogLevel level, object payload) ToLog(this Exception ex)
        {
            var error = ex.ExtractErrorCode();

            var level = MapSeverityLogLevel(error.Severity);

            var payload = new
            {
                Error = error.ToLogObject(),
                Exception = ex.ToLogObject(),
            };
            return (level, payload);
        }

        private static LogLevel MapSeverityLogLevel(ErrorSeverity severity)
        {
            return severity switch
            {
                ErrorSeverity.Low => LogLevel.Information,
                ErrorSeverity.Medium => LogLevel.Warning,
                ErrorSeverity.High => LogLevel.Error,
                ErrorSeverity.Critical => LogLevel.Critical,
                _ => LogLevel.Error
            };
        }
    }
}
