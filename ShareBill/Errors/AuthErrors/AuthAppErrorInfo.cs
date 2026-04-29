namespace ShareBill.Errors.AuthErrors
{
    public class AuthAppErrorInfo : AppErrorInfo
    {
        public required int HttpStatusCode { get; set; }

        /// <summary>
        /// Serializes the error information into an anonymous object for logging purposes, including the HTTP status code specific to authentication errors.
        /// </summary>
        /// <returns></returns>
        public override object ToLogObject() => new
        {
            Code,
            Type,
            Description,
            MessageToShow,
            IsRetryable,
            HttpStatusCode
        };

        /// <summary>
        /// Provides additional information about the current HTTP status code for diagnostic or logging purposes.
        /// </summary>
        /// <returns>A string containing the HTTP status code in a formatted representation.</returns>
        protected override string AdditionalInfo()
        {
            return $"HttpStatusCode: {HttpStatusCode}";
        }
    }
}
