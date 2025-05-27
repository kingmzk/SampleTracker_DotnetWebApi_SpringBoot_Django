using ModelTracKer.Models;

namespace ModelTracKer.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }
}
