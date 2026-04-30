using System.Text;

namespace ShareBill.Errors
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Converts the specified exception to an object suitable for structured logging.
        /// </summary>
        /// <param name="ex">The exception to convert. Cannot be null.</param>
        /// <returns>An object containing details of the exception, formatted for logging purposes.</returns>
        public static object ToLogObject(this Exception ex) 
        {
            if (ex == null) return new { };

            return BuildExceptionObject(ex);
        }

        private static object BuildExceptionObject(Exception ex)
        {
            return new
            {
                Type = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                InnerException = ex.InnerException != null
                    ? BuildExceptionObject(ex.InnerException)
                    : null,
                AggregateExceptions = ex is AggregateException aggEx
                    ? aggEx.InnerExceptions.Select(BuildExceptionObject)
                    : null
            };
        }


        /// <summary>
        /// Creates a detailed message string that includes information from the specified exception and all inner
        /// exceptions.
        /// </summary>
        /// <remarks>This method is useful for logging or displaying comprehensive error information, as
        /// it traverses the entire exception chain.</remarks>
        /// <param name="ex">The exception from which to extract the full message. Can be null.</param>
        /// <returns>A string containing the concatenated messages of the exception and its inner exceptions. Returns an empty
        /// string if <paramref name="ex"/> is null.</returns>
        public static string GetFullExceptionMessage(this Exception ex)
        {
            if (ex == null) return string.Empty;

            StringBuilder message = new StringBuilder();

            AppendException(message, ex, 0);


            return message.ToString();
        }


        private static void AppendException(StringBuilder message, Exception ex, int level)
        {
            var indent = new string(' ', level * 2);

            message.AppendLine($"{indent}[{ex.GetType().FullName}]");
            message.AppendLine($"{indent}Message: {ex.Message}");

            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                message.AppendLine($"{indent}StackTrace: {ex.StackTrace}");
            }

            if (ex is AggregateException aggEx)
            {
                foreach (var inner in aggEx.InnerExceptions)
                {
                    message.AppendLine($"{indent} -- Inner Exception -- ");
                    AppendException(message, inner, level + 1);
                }
                return;
            }

            if (ex.InnerException != null)
            {
                message.AppendLine($"{indent} -- Inner Exception -- ");
                AppendException(message, ex.InnerException, level + 1);
            }
        }
    }
}
