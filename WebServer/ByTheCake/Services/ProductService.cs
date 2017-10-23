namespace WebServer.ByTheCake.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.ByTheCake.Data;
    using WebServer.ByTheCake.Data.Models;
    using WebServer.ByTheCake.Services.Contracts;
    using WebServer.ByTheCake.ViewModels.Products;

    public class ProductService : IProductService
    {
        public void Create(string name, decimal price, string imageUrl)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var product = new Product
                {
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl
                };

                db.Add(product);
                db.SaveChanges();
            }
        }

        public IEnumerable<ProductListingViewModel> All(string searchTerm = null)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var resultsQuery = db.Products.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    resultsQuery = resultsQuery
                        .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()));
                }

                return resultsQuery.Select(pr => new ProductListingViewModel
                {
                     Id = pr.Id,
                     Name = pr.Name,
                     Price = pr.Price
                }).ToList();
            }
        }

        public ProductDetailsViewModel Find(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Where(p => p.Id == id)
                    .Select(pr => new ProductDetailsViewModel
                    {
                        Name = pr.Name,
                        Price = pr.Price,
                        ImageUrl = pr.ImageUrl
                    })
                    .FirstOrDefault();
            }
        }

        public bool Exists(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products.Any(p => p.Id == id);
            }
        }

        public IEnumerable<ProductInCartViewModel> FindProductInCart(IEnumerable<int> ids)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Products
                    .Where(p => ids.Contains(p.Id))
                    .Select(pr => new ProductInCartViewModel
                    {
                        Name = pr.Name,
                        Price = pr.Price
                    })
                    .ToList();
            }
        }
    }
}