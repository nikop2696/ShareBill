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

        /// <summary>
        /// Returns a string containing additional information about the HTTP status code associated with the current
        /// instance.
        /// </summary>
        /// <remarks>This method is intended to provide supplementary diagnostic information for logging
        /// or debugging purposes. The returned string reflects the current value of the HttpStatusCode
        /// property.</remarks>
        /// <returns>A string that includes the HTTP status code in the format "HttpStatusCode: {value}".</returns>
        protected override string AdditionalInfo() 
        {
            return $"HttpStatusCode: {HttpStatusCode}";
        }
    }
}
