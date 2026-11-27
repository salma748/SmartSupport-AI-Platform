namespace SmartSupport_AI.BLL.DTOs.Tickets
{
    public class ChangeTicketStatusDto
    {
        public int TicketId { get; set; }

        public string NewStatus { get; set; } = string.Empty;

        public int ChangedByUserId { get; set; }

        public string? Reason { get; set; }
    }
}