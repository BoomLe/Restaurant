using Microsoft.EntityFrameworkCore;
using Restaurant.Service.ApiCoupons.Models;

namespace Restaurant.Service.ApiCoupons.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option) : base (option)
        {
            
        }

        public DbSet<Coupon> Coupons { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon 
                {
                    CouponId = 1,
                    CouponCode = "Giam10k",
                    DiscountAmount = 10,
                    MinAmount = 20,
                    
                });
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon 
                {
                    CouponId = 2,
                    CouponCode = "Giam20k",
                    DiscountAmount = 20,
                    MinAmount = 40
                });
        }
    }
}
