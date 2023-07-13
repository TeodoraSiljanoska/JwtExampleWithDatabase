﻿using System.Threading.Tasks;
using   JwtExampleWithDatabase.Models;

namespace JwtExampleWithDatabase.Data
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task SaveChangesAsync();
        Task<string> GetUserRoleAsync(string username);
    }
}
