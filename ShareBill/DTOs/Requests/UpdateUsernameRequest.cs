namespace ShareBill.DTOs.Requests
{
    public class UpdateUsernameRequest
    {
        public required Guid id { get; set; }
        public required string username { get; set; }
    }
}
