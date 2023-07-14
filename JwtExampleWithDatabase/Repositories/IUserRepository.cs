using System.Threading.Tasks;
using   JwtExampleWithDatabase.Models;

namespace JwtExampleWithDatabase.Data
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> ValidateLogin(string username,string password);
        Task AddAsync(User user);
        Task SaveChangesAsync();
        Task<string> GetUserRoleAsync(string username);
    }
}
