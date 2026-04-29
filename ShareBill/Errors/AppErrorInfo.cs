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

        protected virtual string AdditionalInfo() => string.Empty;
        

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
