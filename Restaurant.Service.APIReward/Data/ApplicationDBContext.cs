using Microsoft.EntityFrameworkCore;
using Restaurant.Service.APIReward.Models;

namespace Restaurant.Service.APIReward.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option) : base (option)
        {
            
        }

     
        public DbSet<Rewards> Rewards { set; get; }
    }
}
