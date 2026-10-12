namespace JENSENonline.Api.DTOs
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
