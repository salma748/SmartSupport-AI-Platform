namespace SmartSupport_AI.BLL.DTOs.Orders
{
    public class ChangeRefundStatusDto
    {
        public int RefundId { get; set; }

        public string NewStatus { get; set; } = string.Empty;
    }
}
