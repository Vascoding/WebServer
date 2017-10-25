namespace WebServer.ByTheCake.Controllers
{
    using System;
    using System.Linq;
    using WebServer.ByTheCake.Data;
    using WebServer.ByTheCake.Services;
    using WebServer.ByTheCake.Services.Contracts;
    using WebServer.ByTheCake.ViewModels;
    using WebServer.Infrastructure;
    using WebServer.Server.HTTP;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;

    public class ShoppingController : BaseController
    {
        private readonly IUserService users;
        private readonly IProductService products;
        private readonly IShoppingService shopping;

        public ShoppingController()
        {
            this.users = new UserService();
            this.products = new ProductService();
            this.shopping = new ShoppingService();
        }

        public IHttpResponse AddToCart(IHttpRequest request)
        {
            var id = int.Parse(request.UrlParameters["id"]);

            var productExists = this.products.Exists(id);

            if (!productExists)
            {
                return new NotFoundResponse();
            }

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            shoppingCart.ProductIds.Add(id);

            var redirectUrl = "/search";

            const string searchTermKey = "searchTerm";

            if (request.QueryParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={request.QueryParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest req)
        {
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.ProductIds.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }

            else
            {
                var productsInCart = this.products.FindProductInCart(shoppingCart.ProductIds);

                var items = productsInCart
                    .Select(i => $"<div>{i.Name} - {i.Price:f2}$</div><br />");

                this.ViewData["cartItems"] = string.Join(string.Empty, items);

                var totalPrice = productsInCart
                    .Sum(i => i.Price);

                this.ViewData["totalCost"] = $"{totalPrice:f2}";
            }
            
            return this.FileViewResponse(@"shopping\cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest req)
        {
            var username = req.Session.Get<string>(SessionStore.CurrentUserKey);
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);


            var userId = this.users.GetUserId(username);
            if (userId == null)
            {
                throw new InvalidOperationException($"User {username} does not exists!");
            }

            var productIds = shoppingCart.ProductIds;
            if (!productIds.Any())
            {
                return new RedirectResponse("/");
            }

            this.shopping.CreateOrder(userId.Value, productIds);
            
            shoppingCart.ProductIds.Clear();

            return this.FileViewResponse(@"shopping/finish-order");
        }
    }
}