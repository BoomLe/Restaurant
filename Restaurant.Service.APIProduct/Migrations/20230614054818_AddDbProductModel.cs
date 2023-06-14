using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant.Service.APIProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddDbProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Pizza", "Nước, bột và men được để lên men từ 24 giờ trở lên. Nhào nặn thành những chiếc bánh tròn, dẹt rồi cho vào lò nướng chín. Loại bánh này siêu ngon ", "https://placehold.co/603x403", "Pizza Hải Sản Pesto Xanh", 300000.0 },
                    { 2, "Món Nướng", "Cá Chẽm Fillet Đút Lò Sốt Carri Chanh là món ăn đặc biệt làm say mê rất nhiều thực khách bởi hương vị chua chua, mặn mặn, thơm thơm từ chanh muối. Với công thức áp chảo chuyên nghiệp cùng nước sốt cà ri chanh muối lạ vị đã mang đến cho móng ăn được nhiều người yêu thích", "https://placehold.co/602x402", "Cá Phile Đút Lò", 125000.0 },
                    { 3, "Món Chiên", "Mực có vị ngọt tự nhiên, dai dai được tẩm ướp gia vị và phủ lớp bột bên ngoài mang đén cảm giác giòn rụm và đậm đà cho người thưởng thức.", "https://placehold.co/601x401", "Mực chiên giòn", 85000.0 },
                    { 4, "Món chiên", "Chả giò hải sản giòn rụm với nhân hải sản gồm tôm, mực tươi ngon bên trong sẽ là món ăn khai vị hấp dẫn cho các bữa tiệc", "https://placehold.co/600x400", "Chả giò hải sản", 72000.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
