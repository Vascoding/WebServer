namespace WebServer.GameStore.Controllers
{
    using System;
    using System.Linq;
    using WebServer.GameStore.Common;
    using WebServer.GameStore.Services;
    using WebServer.GameStore.Services.Contracts;
    using WebServer.GameStore.ViewModels.Orders;
    using WebServer.Server.HTTP;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;

    public class ShoppingController : BaseController
    {
        private readonly IGameService games;
        private readonly IShoppingService shopping;

        public ShoppingController(IHttpRequest request) 
            : base (request)
        {
            this.games = new GameService();
            this.shopping = new ShoppingService();
        }

        public IHttpResponse CartDetails()
        {
            var shoppingCart = this.Request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.GameIds.Any())
            {
                this.ViewData["gamesInCart"] = string.Empty;
                this.ViewData["totalPrice"] = "0.00";
            }

            else
            {
                var gamesInCart = this.games.GamesInCart(shoppingCart.GameIds);

                var result = gamesInCart.Select(g =>
                    new HtmlHelper().ShoppingCartGamesViewPattern(g.Id, g.Title, g.Description, g.Price, g.Image));

                var gamesAsHtml = string.Join(Environment.NewLine, result);
                this.ViewData["gamesInCart"] = gamesAsHtml;
                var totalPrice = gamesInCart.Sum(p => p.Price).ToString();
                this.ViewData["totalPrice"] = totalPrice;
            }

            return this.FileViewResponse(@"cart\cart");
        }

        public IHttpResponse AddToCart()
        {
            var id = int.Parse(this.Request.UrlParameters["id"]);

            var shoppingCart = this.Request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.GameIds.Contains(id))
            {
                shoppingCart.GameIds.Add(id);
            }
            
            return new RedirectResponse(@"\user\cart");
        }

        public IHttpResponse Order()
        {
            var shoppingCart = this.Request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            var userEmail = this.Request.Session.Get(SessionStore.CurrentUserKey);
            this.shopping.CreateOrder(userEmail.ToString(), shoppingCart.GameIds);
            shoppingCart.GameIds.Clear();
            return new RedirectResponse(@"\");
        }

        public IHttpResponse Dismiss()
        {
            var id = int.Parse(this.Request.UrlParameters["id"]);
            var shoppingCart = this.Request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shoppingCart.GameIds.Remove(id);

            return this.CartDetails();
        }
    }
}