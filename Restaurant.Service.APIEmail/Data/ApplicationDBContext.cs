using Microsoft.EntityFrameworkCore;
using Restaurant.Service.APIEmail.Models;

namespace Restaurant.Service.APIEmail.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option) : base (option)
        {
            
        }

        public DbSet<EmailLogger> EmailLoggers { set; get; }

     
        
    }
}
