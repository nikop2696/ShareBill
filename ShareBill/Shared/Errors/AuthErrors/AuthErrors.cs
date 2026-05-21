using ShareBill.Shared.Errors;

namespace ShareBill.Shared.Errors.AuthErrors
{
    public static class AuthErrors
    {
        public static AuthAppErrorInfo UserAlreadyInUse => new()
        {
            Code = "UserAlreadyInUse",
            Type = ErrorType.Validation,
            Description = "The username is already taken.",
            IsRetryable = false,
            HttpStatusCode = 409,
            Severity = ErrorSeverity.Medium
        };

        public static AuthAppErrorInfo SupabaseInvalidSignUpResponse => new()
        {
            Code = "SupabaseInvalidSignUpResponse",
            Type = ErrorType.Server,
            Severity = ErrorSeverity.High,
            IsRetryable = false,
            HttpStatusCode = 500,
            Description = "Supabase returned an invalid sign-up response."
        };

        public static AuthAppErrorInfo SignUpProfileUpdateFailed => new()
        {
            Code = "SignUpProfileUpdateFailed",
            Type = ErrorType.Server,
            Severity = ErrorSeverity.High,
            IsRetryable = false,
            HttpStatusCode = 502,
            Description = "Failed to update the user profile after sign-up."
        };
        public static AuthAppErrorInfo SignUpUsernameNotFound => new()
        {
            Code = "SignUpUsernameNotFound",
            Type = ErrorType.Server,
            Severity = ErrorSeverity.High,
            IsRetryable = false,
            HttpStatusCode = 404,
            Description = "Failed to find the username after sign-up."
        };
        public static AuthAppErrorInfo UnknownSignUpError => new()
        {
            Code = "UnknownSignUpError",
            Type = ErrorType.Unknown,
            Severity = ErrorSeverity.High,
            IsRetryable = false,
            HttpStatusCode = 500,
            Description = "An unknown error occurred during sign-up."
        };
        public static AuthAppErrorInfo NetworkTimeOut => new()
        {
            Code = "NetworkTimeout",
            Type = ErrorType.Network,
            Severity = ErrorSeverity.Medium,
            IsRetryable = true,
            HttpStatusCode = 0,
            Description = "A network timeout occurred while communicating with the authentication service."
        };
        public static AuthAppErrorInfo NetworkFailure => new()
        {
            Code = "NetworkFailure",
            Type = ErrorType.Network,
            Severity = ErrorSeverity.Medium,
            IsRetryable = true,
            HttpStatusCode = 0,
            Description = "A network failure occurred while communicating with the authentication service."
        };

        public static AuthAppErrorInfo MultipleUser => new()
        {
            Code = "MultipleUser",
            Type = ErrorType.Validation,
            Severity = ErrorSeverity.High,
            IsRetryable = false,
            HttpStatusCode = 422,
            Description = "Multiple profiles found for user."
        };
        public static AuthAppErrorInfo SignInFailed => new()
        {
            Code = "SignInFailed",
            Type = ErrorType.Authentication,
            Severity = ErrorSeverity.Low,
            IsRetryable = false,
            HttpStatusCode = 404,
            Description = "Email or Password Invalid."
        };
        public static AuthAppErrorInfo DeserealizationFailed => new()
        {
            Code = "DeserealizationFailed",
            Type = ErrorType.Server,
            Severity = ErrorSeverity.High,
            IsRetryable = false,
            HttpStatusCode = 404,
            Description = "The deserialization has failed"
        };

    }
}
