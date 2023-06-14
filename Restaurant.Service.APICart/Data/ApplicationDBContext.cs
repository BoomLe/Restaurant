using Microsoft.EntityFrameworkCore;
using Restaurant.Service.APICart.Models;

namespace Restaurant.Service.APICart.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option) : base (option)
        {
            
        }

        public DbSet<CartHeader> CartHeaders { set; get; }
        public DbSet<CartDetails> CartDetails { set; get; }

      
        
    }
}
