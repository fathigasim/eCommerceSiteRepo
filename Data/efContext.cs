using efcoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace efcoreApi.Data
{
    public class efContext : DbContext
    {
        public efContext(DbContextOptions<efContext> opt)
          : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("SequenceOrder");
            modelBuilder.Entity<Goods>().HasIndex(p => p.GoodsName).IsUnique(true);
            modelBuilder.Entity<Orders>().Property(p => p.OrdSeq).HasDefaultValueSql("NEXT VALUE FOR SequenceOrder");
        }
        public DbSet<Category> category { get; set; }
        public DbSet<Goods> goods { get; set; }
        public DbSet<Basket> basket { get; set; }
        public DbSet<BasketItems> basketItems { get; set; }
        public DbSet<PasswordResetToken> passwordResetTokens { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Register> Register { get; set; }

        public DbSet<Orders> Order { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
    }
}
