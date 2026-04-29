using Microsoft.AspNetCore.Mvc;
using ShareBill.Errors;
using ShareBill.Errors.ResponsesError;

namespace ShareBill.DTOs.Responses
{

    public class OperationResult<T> 
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public string Message { get; init; } = string.Empty;
        public ResponseAppErrorInfo? Error { get; init; }

        public static OperationResult<T> Ok(T data, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        public static OperationResult<T> Fail(ResponseAppErrorInfo error, string message = "") =>
            new() { Success = false, Error = error, Message = message };
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
