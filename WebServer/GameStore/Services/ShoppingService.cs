namespace WebServer.GameStore.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.GameStore.Data;
    using WebServer.GameStore.Data.Models;
    using WebServer.GameStore.Services.Contracts;

    public class ShoppingService : IShoppingService
    {
        public void CreateOrder(string userEmail, IEnumerable<int> gameIds)
        {
            using (var db = new GameStoreDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == userEmail);
                
                foreach (var gameId in gameIds)
                {
                    if (!db.UserGame.Any(u => u.UserId == user.Id && u.GameId == gameId))
                    {
                        user.Games.Add(new UserGame
                        {
                            UserId = user.Id,
                            GameId = gameId
                        });
                    }
                }

                db.SaveChanges();
            }
        }
    }
}