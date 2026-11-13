using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public string OrderStatus { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public DateTime PlacedAt { get; set; }

        public Customer Customer { get; set; } = null!;

        public ICollection<OrderStatusCheck> StatusChecks { get; set; } = new List<OrderStatusCheck>();

        public ICollection<RefundRequest> RefundRequests { get; set; } = new List<RefundRequest>();
    }
}
