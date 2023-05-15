using ImageGeneratorApi.Data;
using ImageGeneratorApi.Models;

namespace ImageGeneratorApi.Services
{
    public class UserService
    {
        private readonly ImageGeneratorApiContext _context;

        public UserService(ImageGeneratorApiContext context)
        {
            _context = context;
        }

        public User CreateUser(string email, string password)
        {
            //hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var entityEntry = _context.Users.Add(new User("", email, hashedPassword, "User", true, 1));
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public User CreateGoogleUser(string email, string googleId, string name)
        {
            var entityEntry = _context.Users.Add(new User(googleId, email, "", name, true, 2));
            _context.SaveChanges();

            return entityEntry.Entity;
        }

        public bool IsExistingUser(string email)
        {
            var isExistingUser = _context.Users.Any(u => u.Email == email);

            return isExistingUser;
        }

        public User VerifyUser(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new Exception("Invalid password");
            }

            return user;
        }

        public User GetUserByEmailAndGoogleId(string email, string googleId)
        {
            var userByEmailAndGoogleId = _context.Users.FirstOrDefault(u => u.Email == email && u.GoogleId == googleId);

            if (userByEmailAndGoogleId == null)
            {
                throw new Exception("User not found");
            }

            return userByEmailAndGoogleId;
        }
    }
}