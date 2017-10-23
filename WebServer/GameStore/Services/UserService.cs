namespace WebServer.GameStore.Services
{
    using System.Linq;
    using WebServer.GameStore.Data;
    using WebServer.GameStore.Data.Models;
    using WebServer.GameStore.Services.Contracts;

    public class UserService : IUserService
    {
        public bool Create(string email, string name, string password)
        {
            using (var db = new GameStoreDbContext())
            {
                if (db.Users.Any(e => e.Email == email))
                {
                    return false;
                }

                User user = new User
                {
                    Email = email,
                    Name = name,
                    Password = password
                };
                if (!db.Users.Any())
                {
                    user.IsAdmin = true;
                }
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public bool FindUser(string email, string password)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email == email && u.Password == password);
            }
        }

        public bool IsAdmin(string email)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email == email && u.IsAdmin);
            }
        }
    }
}