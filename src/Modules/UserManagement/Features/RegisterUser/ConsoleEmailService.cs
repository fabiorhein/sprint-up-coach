using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SprintUp.Modules.UserManagement.Domain.Entities;

namespace SprintUp.Modules.UserManagement.Features.RegisterUser
{
    public class ConsoleEmailService : IEmailService
    {
        private readonly ILogger<ConsoleEmailService> _logger;

        public ConsoleEmailService(ILogger<ConsoleEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendValidationEmailAsync(User user)
        {
            _logger.LogInformation($"Validation email sent to {user.Email}");
            return Task.CompletedTask;
        }
    }
}
