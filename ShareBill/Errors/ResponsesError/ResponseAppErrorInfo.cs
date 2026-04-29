namespace ShareBill.Errors.ResponsesError
{
    public class ResponseAppErrorInfo : AppErrorInfo
    {
        public int HttpStatusCode { get; set; }

        protected override string AdditionalInfo() 
        {
            return $"HttpStatusCode: {HttpStatusCode}";
        }
    }
}
