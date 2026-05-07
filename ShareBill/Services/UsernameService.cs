using ShareBill.Infrastructure.Database;

namespace ShareBill.Services
{
    public class UsernameService
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly ILogger<HealthService> _logger;

        public UsernameService(IDbConnectionFactory dbFactory, ILogger<HealthService> logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }
        //TODO: When the user signUp the insert of the username can fail, even with retry, as it's a different table separeted from Auth.
        //To avoid this but allow the user to login and not be withouth an username
        //At the access we ask the user to set the username.
        //The actual logIn wi
        public async Task<bool> IsUsernameSet()
    }
}
