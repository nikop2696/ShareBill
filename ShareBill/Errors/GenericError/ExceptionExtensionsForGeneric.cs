using System.Runtime.CompilerServices;

namespace ShareBill.Errors.GenericError
{
    public static  class ExceptionExtensionsForGeneric
    {
        public static AppErrorInfo ExtractErrorCode(this Exception ex)
        {
            return GenericErrorResolver.Resolve(ex);
        }
    }
}
