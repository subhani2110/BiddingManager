using DataLayer.Entities;
using DataLayer.Services;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository
{
    public interface IAuthRepository
    {
        Task<User> LoginAsync(string username, string password);
        Task<User> RegisterAsync(User user, string password);
        Task<bool> UserExistsAsync(string username);
        Task<bool> ResetPasswordAsync(string email);
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (user.Password != password)
                return null;

            return user;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return false;

            user.Password = "123Password1$";

            //await _emailService.SendEmailAsync(user.Email, "Password Reset Request", emailBody);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
