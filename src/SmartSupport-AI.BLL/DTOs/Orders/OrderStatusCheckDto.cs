namespace SmartSupport_AI.BLL.DTOs.Orders
{
    public class OrderStatusCheckDto
    {
        public int CheckId { get; set; }

        public int OrderId { get; set; }

        public int ConversationId { get; set; }

        public int? ExecutionId { get; set; }

        public string StatusReturned { get; set; } = string.Empty;

        public DateTime CheckedAt { get; set; }
    }
}
