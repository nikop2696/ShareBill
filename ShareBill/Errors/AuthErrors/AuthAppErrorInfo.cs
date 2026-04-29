namespace ShareBill.Errors.AuthErrors
{
    public class AuthAppErrorInfo : AppErrorInfo
    {
        public required int HttpStatusCode { get; set; }

        protected override string AdditionalInfo()
        {
            return $"HttpStatusCode: {HttpStatusCode}";
        }
    }
}
