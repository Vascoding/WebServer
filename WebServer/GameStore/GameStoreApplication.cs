namespace WebServer.GameStore
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using WebServer.GameStore.ViewModels.Account;
    using WebServer.GameStore.Controllers;
    using WebServer.GameStore.Data;
    using WebServer.GameStore.ViewModels.Games;
    using WebServer.Server.Contracts;
    using WebServer.Server.Handlers;
    using WebServer.Server.Routing.Contracts;

    public class GameStoreApplication : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new GameStoreDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.AnonymousPaths.Add("/");
            appRouteConfig.AnonymousPaths.Add("/register");
            appRouteConfig.AnonymousPaths.Add("/login");

            appRouteConfig
                .AddRoute("/", new GetHandler(req => new HomeController(req).Index()));
            
            appRouteConfig
                .AddRoute("/register", new GetHandler(req => new AccountController(req).Register()));

            appRouteConfig
                .AddRoute("/register", new PostHandler(req => new AccountController(req).Register(new RegisterViewModel
                {
                    Email = req.FormData["email"],
                    FullName = req.FormData["fullName"],
                    Password = req.FormData["password"],
                    ConfirmPassword = req.FormData["confirmPassword"]
                })));

            appRouteConfig
                .AddRoute("/login", new GetHandler(req => new AccountController(req).Login()));

            appRouteConfig
                .AddRoute("/login", new PostHandler(req => new AccountController(req).Login(new LoginUserViewModel
                {
                    Email = req.FormData["email"],
                    Password = req.FormData["password"]
                })));

            appRouteConfig
                .AddRoute("/logout", new GetHandler(req => new AccountController(req).Logout()));

            appRouteConfig
                .AddRoute("/admin/games/add", new GetHandler(req => new AdminController(req).AddGame()));

            appRouteConfig
                .AddRoute("/admin/games/add", new PostHandler(req => new AdminController(req).AddGame(new AddGameViewModel
                {
                    Title = req.FormData["title"],
                    Description = req.FormData["description"],
                    Image = req.FormData["image"],
                    Price = decimal.Parse(req.FormData["price"]),
                    Size = double.Parse(req.FormData["size"]),
                    VideoId = req.FormData["videoId"],
                    RealeaseDate = req.FormData["releaseDate"]
                })));

            appRouteConfig
                .AddRoute("/admin/games/list", new GetHandler(req => new AdminController(req).AllGames()));

            appRouteConfig
                .AddRoute("/admin/games/edit/{(?<id>[0-9]+)}", new GetHandler(req => new AdminController(req).Edit()));

            appRouteConfig
                .AddRoute("/admin/games/edit/{(?<id>[0-9]+)}", new PostHandler(req => new AdminController(req).Edit(new AdminEditGameViewModel
                {
                    Title = req.FormData["title"],
                    Description = req.FormData["description"],
                    Image = req.FormData["image"],
                    Price = decimal.Parse(req.FormData["price"]),
                    Size = double.Parse(req.FormData["size"]),
                    VideoId = req.FormData["videoId"],
                    RealeaseDate = req.FormData["releaseDate"]
                })));

            appRouteConfig
                .AddRoute("/admin/games/delete/{(?<id>[0-9]+)}", new GetHandler(req => new AdminController(req).Delete()));

            appRouteConfig
                .AddRoute("/admin/games/delete/{(?<id>[0-9]+)}", new PostHandler(req => new AdminController(req).Delete(int.Parse(req.UrlParameters["id"]))));

            appRouteConfig
                .AddRoute("/games/details/{(?<id>[0-9]+)}", new GetHandler(req => new HomeController(req).Details()));

           appRouteConfig
                .AddRoute("/user/cart", new GetHandler(req => new ShoppingController(req).CartDetails()));

            appRouteConfig
                .AddRoute("/user/cart", new PostHandler(req => new ShoppingController(req).Order()));

            appRouteConfig
                .AddRoute("/user/cart/{(?<id>[0-9]+)}", new GetHandler(req => new ShoppingController(req).Dismiss()));

            appRouteConfig
                .AddRoute("/user/cart/add/{(?<id>[0-9]+)}", new GetHandler(req => new ShoppingController(req).AddToCart()));

            
        }
    }
}