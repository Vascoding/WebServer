namespace WebServer.ByTheCake.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ByTheCake.Models;
    using Server.HTTP.Contracts;
    using WebServer.ByTheCake.Data;
    using WebServer.ByTheCake.Services;
    using WebServer.ByTheCake.Services.Contracts;
    using WebServer.ByTheCake.ViewModels.Products;
    using WebServer.Infrastructure;
    using WebServer.Server.HTTP.Response;

    public class ProductsController : BaseController
    {
        private readonly IProductService products;

        public ProductsController()
        {
            this.products = new ProductService();
        }

        public IHttpResponse Add()
        {
            this.SetDefaultViewData();
            this.ViewData["sowResult"] = "none";
            return this.FileViewResponse(@"products\add");
        }

        public IHttpResponse Add(AddProductViewModel model)
        {
            this.SetDefaultViewData();

            if (model.Name.Length < 3
                || model.Name.Length > 30
                || model.ImageUrl.Length < 3
                || model.ImageUrl.Length > 2000)
            {
                this.AddError("Product information is not valid.");
                return this.FileViewResponse(@"products\add");
            }

            this.products.Create(model.Name, model.Price, model.ImageUrl);

            
            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = model.Price.ToString();
            this.ViewData["imageUrl"] = model.ImageUrl;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(@"products\add");
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            const string searchTermKey = "searchTerm";
            
            var urlParameters = request.QueryParameters;

            
            this.ViewData["results"] = string.Empty;

            var searchTerm = urlParameters.ContainsKey(searchTermKey) ? urlParameters[searchTermKey] : null;

            this.ViewData["searchTerm"] = searchTerm;

            var result = this.products.All(searchTerm);

            if (!result.Any())
            {
                this.ViewData["results"] = "No cakes found";
            }

            else
            {
                var allProducts = result
                    .Select(c =>
                        $@"<div><a href=""/cakes/{c.Id}"">{c.Name}</a> - ${c.Price:f2} <a href=""/shopping/add/{c.Id}?searchTerm={
                                searchTerm
                            }"">Order</a> </div>");
                var allProductsAsString = string.Join(Environment.NewLine, allProducts);

                this.ViewData["results"] = allProductsAsString;
            }
            
            this.ViewData["showCart"] = "none";

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.ProductIds.Any())
            {
                var totalProducts = shoppingCart.ProductIds.Count;
                var totalProductsString = shoppingCart.ProductIds.Count != 1 ? "products" : "product";
                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {totalProductsString}";
            }

            return this.FileViewResponse(@"products\search");
        }

        private void SetDefaultViewData()
        {
            this.ViewData["showError"] = "none";
            this.ViewData["error"] = "none";
        }

        public IHttpResponse Details(int id)
        {
            var product = this.products.Find(id);

            if (product == null)
            {
                return new NotFoundResponse();
            }

            this.ViewData["name"] = product.Name;
            this.ViewData["price"] = product.Price.ToString("f2");
            this.ViewData["imageUrl"] = product.ImageUrl;

            return this.FileViewResponse(@"products\details");
        }
    }
}