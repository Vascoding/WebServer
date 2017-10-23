namespace WebServer.ByTheCake
{
    using Server.Contracts;
    using Server.Routing.Contracts;
    using ByTheCake.Controllers;
    using Microsoft.EntityFrameworkCore;
    using Server.Handlers;
    using WebServer.ByTheCake.Data;
    using WebServer.ByTheCake.ViewModels.Account;
    using WebServer.ByTheCake.ViewModels.Products;


    public class ByTheCakeApplication : IApplication
    {
        public void InitializeDatabase()
        {
            using (var db = new ByTheCakeDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .AddRoute("/", new GetHandler(req => new HomeController().Index()));

            appRouteConfig
                .AddRoute("/about", new GetHandler(req => new HomeController().About()));

            appRouteConfig
                .AddRoute("/add", new GetHandler(req => new ProductsController().Add()));

            appRouteConfig
                .AddRoute("/add", new PostHandler(req => new ProductsController().Add(new AddProductViewModel
                {
                    Name = req.FormData["name"],
                    Price = decimal.Parse(req.FormData["price"]),
                    ImageUrl = req.FormData["imageUrl"]
                })));

            appRouteConfig
                .AddRoute("/search", new GetHandler(req => new ProductsController().Search(req)));
            
            appRouteConfig
                .AddRoute("/calculator", new GetHandler(req => new CalculatorController().Calculate(req.QueryParameters)));

            appRouteConfig
                .AddRoute("/login", new GetHandler(req => new LoginController().Login()));

            appRouteConfig
                .AddRoute("/login", new PostHandler(req => new LoginController().Login(req, new LoginViewModel
                {
                    Username = req.FormData["username"],
                    Password = req.FormData["password"]
                })));

            appRouteConfig
                .AddRoute("/logout", new PostHandler(req => new LoginController().Logout(req)));

            appRouteConfig
                .AddRoute("/register", new GetHandler(req => new LoginController().Register()));

            appRouteConfig
                .AddRoute("/register", new PostHandler(req => new LoginController().Register(
                    req, 
                    new RegisterUserVIewModel
                {
                    Username = req.FormData["username"],
                    Password = req.FormData["password"],
                    ConfirmPassword = req.FormData["confirm-password"]
                })));

            appRouteConfig
                .AddRoute("/shopping/add/{(?<id>[0-9]+)}", new GetHandler(req => new ShoppingController().AddToCart(req)));

            appRouteConfig
                .AddRoute("/cart", new GetHandler(req => new ShoppingController().ShowCart(req)));

            appRouteConfig
                .AddRoute("/shopping/finish-order", new PostHandler(req => new ShoppingController().FinishOrder(req)));

            appRouteConfig
                .AddRoute("/profile", new GetHandler(req => new LoginController().Profile(req)));

            appRouteConfig
                .AddRoute("/cakes/{(?<id>[0-9]+)}", new GetHandler(req => new ProductsController().Details(int.Parse(req.UrlParameters["id"]))));
        }
    }
}