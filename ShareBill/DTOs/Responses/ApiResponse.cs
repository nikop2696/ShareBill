using Microsoft.AspNetCore.Mvc;
using ShareBill.Errors;
using ShareBill.Errors.ResponsesError;
using ShareBill.Errors.GenericError;

namespace ShareBill.DTOs.Responses
{

    public class OperationResult<T> 
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public string Message { get; init; } = string.Empty;
        public string ErrorCode { get; init; } = string.Empty;

        public static OperationResult<T> Ok(T data, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        public static OperationResult<T> Fail(Exception ex) 
        {
            var error = ex.ExtractErrorCode();

            return new()
            {
                Success = false,
                Message = string.IsNullOrWhiteSpace(error.MessageToShow) ? "An error occurred." : error.MessageToShow,
                ErrorCode = error.Code,
            };
        }

    }


    public class AuthResponse
    {
        public Guid UserID { get; set; } 
    }

    public class UserResponse
    {
        public Guid UserID { get; set; } 
        public string UserName { get; set; } = string.Empty;
    }
    public class  SignUpResponse
    {
    }
}
