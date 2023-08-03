using GymApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GymApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<AppUser> AppUsers  { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealProduct> MealProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MealProduct>().HasKey(mp => new { mp.MealId, mp.ProductId });
            modelBuilder.Entity<MealProduct>()
                .HasOne(m => m.Meal)
                .WithMany(mp => mp.MealProducts)
                .HasForeignKey(m => m.MealId);
            modelBuilder.Entity<MealProduct>()
                .HasOne(p => p.Product)
                .WithMany(mp => mp.MealProducts)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
