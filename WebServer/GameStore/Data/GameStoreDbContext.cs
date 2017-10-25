namespace WebServer.GameStore.Data
{
    using Microsoft.EntityFrameworkCore;
    using WebServer.GameStore.Data.Models;

    public class GameStoreDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<UserGame> UserGame { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-924HS8U\SQLEXPRESS;Database=GameStoreDb;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGame>()
                .HasKey(ug => new {ug.UserId, ug.GameId});

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(g => g.Games)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Game>()
                .HasMany(u => u.Users)
                .WithOne(g => g.Game)
                .HasForeignKey(g => g.GameId);
        }
    }
}