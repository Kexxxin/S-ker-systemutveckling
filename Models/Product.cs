namespace JENSENonline.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public List<OrderItem> OrderItems { get; set; } = new();
    }

}

