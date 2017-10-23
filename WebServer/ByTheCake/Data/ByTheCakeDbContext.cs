namespace WebServer.ByTheCake.Data
{
    using Microsoft.EntityFrameworkCore;
    using WebServer.ByTheCake.Data.Models;
    using WebServer.ByTheCake.Models;

    public class ByTheCakeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-924HS8U\SQLEXPRESS;Database=ByTheCakeDb;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(o => o.Orders)
                .WithOne(u => u.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<Product>()
                .HasMany(o => o.Orders)
                .WithOne(p => p.Product)
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<Order>()
                .HasMany(p => p.Products)
                .WithOne(o => o.Order)
                .HasForeignKey(p => p.OrderId);
        }
    }
}