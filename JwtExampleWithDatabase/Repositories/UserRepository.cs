using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JwtExampleWithDatabase.Models;
using JwtExampleWithDatabase.Data;

namespace JwtExampleWithDatabase.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
           
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> ValidateLogin(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetUserRoleAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user?.Role;
        }
    }
}
