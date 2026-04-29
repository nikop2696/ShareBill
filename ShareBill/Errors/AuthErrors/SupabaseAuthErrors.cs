using Polly.Telemetry;

namespace ShareBill.Errors.AuthErrors
{
    public static class SupabaseAuthErrors
    {
        /// <summary>
        /// Dictionary of the handled Errors from Supabase with their corresponding AppErrorInfo.
        /// The key is the error code from Supabase and the value is the AppErrorInfo that contains the details of the error.
        /// To UPDATE
        /// </summary>
        public static readonly Dictionary<string, AuthAppErrorInfo> ErrorMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Unknown"] = new()
                {
                    Code = "Unknown",
                    Type = ErrorType.Unknown,
                    Description = "The reason for the error could not be determined.",
                    IsRetryable = false,
                    HttpStatusCode = 0
                },
                ["Offline"] = new()
                {
                    Code = "Offline",
                    Type = ErrorType.Network,
                    Description = "The client is set to run offline or the network is unavailable.",
                    IsRetryable = true,
                    HttpStatusCode = 0
                },
                ["UserEmailNotConfirmed"] = new()
                {
                    Code = "UserEmailNotConfirmed",
                    Type = ErrorType.Validation,
                    Description = "The user's email address has not been confirmed.",
                    MessageToShow = "Please confirm your email address before trying to log in.",
                    IsRetryable = false,
                    HttpStatusCode = 0
                },
                ["UserBadMultiple"] = new()
                {
                    Code = "UserBadMultiple",
                    Type = ErrorType.Validation,
                    Description = "The user's email address and password are invalid.",
                    MessageToShow = "The email address or password you entered is incorrect. Please try again.",
                    IsRetryable = false,
                    HttpStatusCode = 0
                },
                ["UserBadPassword"] = new()
                {
                    Code = "UserBadPassword",
                    Type = ErrorType.Validation,
                    Description = "The user's password is invalid.",
                    MessageToShow = "The password you entered is incorrect. Please try again.",
                    IsRetryable = false,
                    HttpStatusCode = 0
                },
                ["UserBadLogin"] = new()
                {
                    Code = "UserBadLogin",
                    Type = ErrorType.Validation,
                    Description = "The user's login is invalid.",
                    MessageToShow = "The login information you entered is incorrect. Please try again.",
                    IsRetryable = false,
                    HttpStatusCode = 0
                },
                ["UserBadEmailAddress"] = new()
                {
                    Code = "UserBadEmailAddress",
                    Type = ErrorType.Validation,
                    Description = "The user's email address is invalid.",
                    MessageToShow = "The email address you entered is invalid. Please check the format and try again.",
                    IsRetryable = false,
                    HttpStatusCode = 0
                },
                 ["UserTooManyRequests"] = new()
                {
                    Code = "UserTooManyRequests",
                    Type = ErrorType.Network,
                    Description = "Server rejected due to number of requests",
                    MessageToShow = "Too many attempts. Please wait a moment before trying again.",
                    IsRetryable = false,
                     HttpStatusCode = 0
                 },
                 ["UserAlreadyRegistred"] = new() 
                 {
                    Code = "UserAlreadyRegistred",
                    Type = ErrorType.Validation,
                    Description = "The user is already registered.",
                    MessageToShow = "A user with this email address already exists. Please sign up with a different email address.",
                    IsRetryable = false,
                     HttpStatusCode = 0
                 }

            };
            
        
    }
}
