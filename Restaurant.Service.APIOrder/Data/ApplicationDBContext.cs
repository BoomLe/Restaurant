using Microsoft.EntityFrameworkCore;
using Restaurant.Service.APIOrder.Models;

namespace Restaurant.Service.APIOrder.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option) : base (option)
        {
            
        }

        public DbSet<OrderDetails> OrderDetails { set; get; }
        public DbSet<OrderHeader> OrderHeaders { set; get; }

      
        
    }
}
