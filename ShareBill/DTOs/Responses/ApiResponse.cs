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

        /// <summary>
        /// Creates a successful operation result containing the specified data and an optional message.
        /// </summary>
        /// <param name="data">The data to include in the operation result. This value is assigned to the Data property of the result.</param>
        /// <param name="message">An optional message describing the result. If not specified, the message is set to an empty string.</param>
        /// <returns>An OperationResult<T> instance representing a successful operation, containing the provided data and
        /// message.</returns>
        public static OperationResult<T> Ok(T data, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        /// <summary>
        /// Creates a failed operation result with the specified error message and optional error code.
        /// </summary>
        /// <param name="message">The error message that describes the reason for the failure. Cannot be null.</param>
        /// <param name="errorCode">An optional error code that identifies the type of failure. If not specified, the error code is set to an
        /// empty string.</param>
        /// <returns>An instance of OperationResult<T> representing a failed operation, with Success set to false, and the
        /// specified message and error code.</returns>
        public static OperationResult<T> Fail(string message, string errorCode = "") =>
            new() { Success = false, Message = message, ErrorCode = errorCode };
        /// <summary>
        /// Creates a failed operation result with the specified exception information.
        /// </summary>
        /// <remarks>The returned result will have Success set to <see langword="false"/>. If the
        /// exception does not provide a user-friendly message, a default message is used.</remarks>
        /// <param name="ex">The exception that caused the operation to fail. Cannot be null.</param>
        /// <returns>An OperationResult<T> instance representing a failed operation, containing the extracted error code and a
        /// user-friendly error message.</returns>
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

        /// <summary>
        /// Creates a failed operation result with the specified error information.
        /// </summary>
        /// <remarks>If <paramref name="errorInfo"/> does not specify a message to show, a default error
        /// message is used. Use this method to standardize error handling and reporting in operation results.</remarks>
        /// <param name="errorInfo">The error details to include in the failed result. Cannot be null. The error information determines the
        /// error code and the message presented to the user.</param>
        /// <returns>An instance of <see cref="OperationResult{T}"/> representing a failed operation, populated with the provided
        /// error information.</returns>
        public static OperationResult<T> Fail(AppErrorInfo errorInfo) =>
            new()
            {
                Success = false,
                Message = string.IsNullOrWhiteSpace(errorInfo.MessageToShow) ? "An error occurred." : errorInfo.MessageToShow,
                ErrorCode = errorInfo.Code,
            };

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
