namespace SmartSupport_AI.BLL.DTOs.Orders
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public string OrderStatus { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public DateTime PlacedAt { get; set; }
    }
}
