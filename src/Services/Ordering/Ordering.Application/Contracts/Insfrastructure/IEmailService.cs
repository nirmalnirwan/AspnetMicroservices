using Ordering.Application.Models;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Insfrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
