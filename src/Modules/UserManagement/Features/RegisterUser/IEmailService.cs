using System.Threading.Tasks;
using SprintUp.Modules.UserManagement.Domain.Entities;

namespace SprintUp.Modules.UserManagement.Features.RegisterUser
{
    public interface IEmailService
    {
        Task SendValidationEmailAsync(User user);
    }
}
