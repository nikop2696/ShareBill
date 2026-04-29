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
