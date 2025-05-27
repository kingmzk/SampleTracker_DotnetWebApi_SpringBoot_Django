using Microsoft.EntityFrameworkCore;
using ModelTracKer.Data;
using ModelTracKer.Models;
using ModelTracKer.Services.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ModelTracKer.Services
{
    public class UserService : IUserService
    {
        private readonly TrackerDbContext _context;

        public UserService(TrackerDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return null;

            var passwordHash = ComputeHash(password);
            return user.Password == password ? user : null;
        }

        private string ComputeHash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
