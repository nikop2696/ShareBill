namespace ShareBill.DTOs.Responses
{
    
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class AuthResponse : BaseResponse
    {
        public Guid UserID { get; set; } 
    }

    public class UserResponse : BaseResponse
    {
        public Guid UserID { get; set; } 
        public string UserName { get; set; } = string.Empty;
    }
    public class  SignUpResponse : BaseResponse 
    {
    }
}
