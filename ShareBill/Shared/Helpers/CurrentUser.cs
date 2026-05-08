using System.Security.Claims;

namespace ShareBill.Shared.Helpers
{
    public interface ICurrentUserService
    {
        string UserId { get; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub") ?? throw new UnauthorizedAccessException("User is not authenticated.");
    }
}
