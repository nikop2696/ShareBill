using System.Text;

namespace ShareBill.Errors
{
    public class AppErrorInfo
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
        public string MessageToShow { get; set; } = string.Empty;
        public required ErrorType Type { get; set; }
        public required ErrorSeverity Severity { get; set; }
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
        Unknown,
        Security,
        IO,
        Timeout
    }

    public enum ErrorSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
}
