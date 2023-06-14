using Microsoft.EntityFrameworkCore;
using Restaurant.Service.APIProduct.Models;

namespace Restaurant.Service.APIProduct.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option) : base (option)
        {
            
        }

        public DbSet<Product> Products { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Pizza Hải Sản Pesto Xanh",
                Price = 300000,
                Description = "Nước, bột và men được để lên men từ 24 giờ trở lên. Nhào nặn thành những chiếc bánh tròn, dẹt rồi cho vào lò nướng chín. Loại bánh này siêu ngon ",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Pizza"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Cá Phile Đút Lò",
                Price = 125000,
                Description = "Cá Chẽm Fillet Đút Lò Sốt Carri Chanh là món ăn đặc biệt làm say mê rất nhiều thực khách bởi hương vị chua chua, mặn mặn, thơm thơm từ chanh muối. Với công thức áp chảo chuyên nghiệp cùng nước sốt cà ri chanh muối lạ vị đã mang đến cho móng ăn được nhiều người yêu thích",
                ImageUrl = "https://placehold.co/602x402",
                CategoryName = "Món Nướng"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Mực chiên giòn",
                Price = 85000,
                Description = "Mực có vị ngọt tự nhiên, dai dai được tẩm ướp gia vị và phủ lớp bột bên ngoài mang đén cảm giác giòn rụm và đậm đà cho người thưởng thức.",
                ImageUrl = "https://placehold.co/601x401",
                CategoryName = "Món Chiên"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Chả giò hải sản",
                Price = 72000,
                Description = "Chả giò hải sản giòn rụm với nhân hải sản gồm tôm, mực tươi ngon bên trong sẽ là món ăn khai vị hấp dẫn cho các bữa tiệc",
                ImageUrl = "https://placehold.co/600x400",
                CategoryName = "Món chiên"
            });
        }
    }
}
