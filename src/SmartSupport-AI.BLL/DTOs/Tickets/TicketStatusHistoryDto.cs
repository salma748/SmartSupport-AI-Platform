namespace SmartSupport_AI.BLL.DTOs.Tickets
{
    public class TicketStatusHistoryDto
    {
        public int HistoryId { get; set; }

        public int TicketId { get; set; }

        public int ChangedByUserId { get; set; }

        public string OldStatus { get; set; } = string.Empty;

        public string NewStatus { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }
    }
}