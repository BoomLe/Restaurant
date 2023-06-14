using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Service.APIOrder.Models.Dto
{
    public class OrderDetailsDto
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }  
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { set; get; }
        public double Price { set; get; }


    }
}
