using System.Text;

namespace ShareBill.Errors
{
    public class AppErrorInfo
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
        public string MessageToShow { get; set; } = string.Empty;
        public required ErrorType Type { get; set; }
        // public ErrorSeverity Severity { get; set; }
        public required bool IsRetryable { get; set; }


        /// <summary>
        /// Creates an object containing key error details for logging purposes.
        /// </summary>
        /// <remarks>The returned object is intended for structured logging scenarios where capturing
        /// essential error information is required. The specific properties included are Code, Type, Description, and
        /// IsRetryable.</remarks>
        /// <returns>An anonymous object with properties for the error code, type, description, and retryable status.</returns>
        public virtual object ToLogObject() => new
        {
            Code,
            Type,
            IsRetryable,
            Description
        };
        

        protected virtual string AdditionalInfo() => string.Empty;
        /// <summary>
        /// Lets you get a string representation of the error, including code, type, description, and any additional info provided by derived classes.
        /// This can be useful for simple logging or debugging purposes.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"Code: {Code}, Type: {Type}");

            var additionainfo = AdditionalInfo();

            if (!string.IsNullOrWhiteSpace(additionainfo)) 
            {
                sb.Append($", {additionainfo}");
            }
            
            sb.Append($", isRetryable: {IsRetryable}");
            sb.AppendLine($"Description:");
            sb.AppendLine(Description);
            return sb.ToString();
        }
    }



    // You can expand this enum with more specific error types as needed
    //TODO Need to be more specific
    public enum ErrorType
    {
        Authentication,
        Network,
        Server,
        Client,
        Validation,
        Unknown
    }

    /*public enum ErrorSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }*/
}
