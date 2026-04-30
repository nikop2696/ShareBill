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

    }
}
