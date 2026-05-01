using ShareBill.Errors.GenericError;
using Supabase.Gotrue.Exceptions;
using System.Runtime.CompilerServices;

namespace ShareBill.Errors
{
    public static class ErrorLoggigExtensions
    {
        /// <summary>
        /// Log an exception with its associated error information.
        /// The log level is determined by the error severity, and the payload includes both the error details and the exception information.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <returns>A tuple containing the log level and the payload object.</returns>
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

        /// <summary>
        /// Converts a GotrueException into a log entry with an appropriate log level based on the error severity.
        /// </summary>
        /// <param name="goTrueEX">The GotrueException to log.</param>
        /// <returns>A tuple containing the log level and the payload object.</returns>
        public static (LogLevel level, object payload) ToLog(this GotrueException goTrueEX)
        {
            var error = goTrueEX.ExtractErrorCode();

            var level = MapSeverityLogLevel(error.Severity);

            var payload = new
            {
                Error = error.ToLogObject(),
                Exception = goTrueEX.ToLogObject(),
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
