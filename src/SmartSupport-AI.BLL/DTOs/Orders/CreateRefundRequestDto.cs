namespace SmartSupport_AI.BLL.DTOs.Orders
{
    public class CreateRefundRequestDto
    {
        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public int ConversationId { get; set; }

        public int? ExecutionId { get; set; }

        public decimal Amount { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
