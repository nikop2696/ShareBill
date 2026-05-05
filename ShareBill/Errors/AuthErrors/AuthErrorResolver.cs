using Microsoft.IdentityModel.Tokens;
using Supabase.Gotrue.Exceptions;

namespace ShareBill.Errors.AuthErrors
{
    public static class AuthErrorResolver
    {
        public static AuthAppErrorInfo Resolve(string errorCode) 
            => SupabaseAuthErrors.ErrorMap.TryGetValue(errorCode, out var errorInfo)
            ? errorInfo
            : Uknown();

        // Return a default error info for unknown errors
        private static AuthAppErrorInfo Uknown() => AuthErrors.UnknownSignUpError;

        public static AuthAppErrorInfo FromException(Exception ex)
            => ex switch
            {
                TimeoutException => AuthErrors.NetworkTimeOut,
                HttpRequestException => AuthErrors.NetworkFailure,
                GotrueException gex => Resolve(gex.Reason.ToString()),
                _ => Uknown()
            };


        
    }
}
