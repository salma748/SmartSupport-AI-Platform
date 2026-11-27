namespace SmartSupport_AI.BLL.DTOs.Tickets
{
    public class AddInternalNoteDto
    {
        public int TicketId { get; set; }

        public int AgentId { get; set; }

        public string Content { get; set; } = string.Empty;
    }
}