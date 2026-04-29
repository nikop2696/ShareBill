using System.Text;

namespace ShareBill.Errors
{
    public static class ExceptionExtensions
    {
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
