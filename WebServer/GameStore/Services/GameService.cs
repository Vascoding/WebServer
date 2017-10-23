namespace WebServer.GameStore.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.GameStore.Data;
    using WebServer.GameStore.Data.Models;
    using WebServer.GameStore.Services.Contracts;
    using WebServer.GameStore.ViewModels.Games;

    public class GameService : IGameService
    {
        public void Create(string title, string description, string image, decimal price, double size, string videoId,
            DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
            {
                Game game = new Game
                {
                    Title = title,
                    Description = description,
                    Image = image,
                    Price = price,
                    Size = size,
                    VideoId = videoId,
                    RealeaseDate = releaseDate
                };

                db.Games.Add(game);
                db.SaveChanges();
            }
        }

        public IEnumerable<AdminListGameViewModel> All()
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games
                    .Select(g => new AdminListGameViewModel
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Size = g.Size,
                        Price = g.Price
                    }).ToList();
            }
        }

        public AdminEditGameViewModel Get(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games
                    .Where(i => i.Id == id)
                    .Select(g => new AdminEditGameViewModel
                    {
                        Title = g.Title,
                        Description = g.Description,
                        Price = g.Price,
                        Size = g.Size,
                        Image = g.Image,
                        VideoId = g.VideoId,
                        RealeaseDate = g.RealeaseDate.ToString()
                    }).FirstOrDefault();
            }
        }

        public void Edit(int id, AdminEditGameViewModel model)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games
                    .FirstOrDefault(gid => gid.Id == id);
                game.Title = model.Title;
                game.Price = model.Price;
                game.Description = model.Description;
                game.Image = model.Image;
                game.Size = model.Size;
                game.RealeaseDate = DateTime.Parse(model.RealeaseDate);
                game.VideoId = model.VideoId;

                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.FirstOrDefault(g => g.Id == id);
                db.Games.Remove(game);
                db.SaveChanges();
            }
        }

        public IEnumerable<HomePageListGamesViewMoodel> HomeListGames()
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games
                    .Select(g => new HomePageListGamesViewMoodel
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Size = g.Size,
                        Price = g.Price,
                        Description = g.Description,
                        Image = g.Image
                    }).ToList();
            }
        }

        public AddGameViewModel Details(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games
                    .Where(gid => gid.Id == id)
                    .Select(g => new AddGameViewModel
                    {
                        Title = g.Title,
                        Price = g.Price,
                        Image = g.Image,
                        Description = g.Description,
                        RealeaseDate = g.RealeaseDate.ToString(),
                        Size = g.Size,
                        VideoId = g.VideoId
                    })
                    .FirstOrDefault();
            }
        }
    }
}