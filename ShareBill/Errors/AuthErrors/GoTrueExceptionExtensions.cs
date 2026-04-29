using Supabase.Gotrue.Exceptions;

namespace ShareBill.Errors.AuthErrors

{
    public static class GoTrueExceptionExtensions
    {
        public static AppErrorInfo ExtractErrorCode(this Supabase.Gotrue.Exceptions.GotrueException goTrueEx)
        {
            try
            {
                return AuthErrorResolver.Resolve(goTrueEx.Reason.ToString());
            }
            catch (Exception ex)
            {

                return AuthErrorResolver.FromException(ex);
            }
        }
    }
}
