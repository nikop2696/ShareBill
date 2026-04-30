namespace ShareBill.Errors.ResponsesError
{
    public class ResponseAppErrorInfo : AppErrorInfo
    {
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// Serializes the error information into an anonymous object for logging purposes, including the HTTP status code specific to response errors.
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
