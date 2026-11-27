namespace SmartSupport_AI.BLL.DTOs.Orders
{
    public class CheckOrderStatusDto
    {
        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public int ConversationId { get; set; }

        public int? ExecutionId { get; set; }
    }
}
