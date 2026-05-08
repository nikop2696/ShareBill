using Supabase.Gotrue.Exceptions;

namespace ShareBill.Shared.Errors.AuthErrors

{
    public static class GoTrueExceptionExtensions
    {
        public static AuthAppErrorInfo ExtractErrorCode(this Supabase.Gotrue.Exceptions.GotrueException goTrueEx)
        {
            try
            {
                return AuthErrorResolver.Resolve(goTrueEx.Reason.ToString());
            }
            catch (Exception ex)
            {

                return AuthErrorResolver.FromException(new Exception("Failed to extract GoTrue error code.", ex));
            }
        }
    }
}
