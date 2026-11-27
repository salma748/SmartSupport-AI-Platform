namespace SmartSupport_AI.BLL.DTOs.Tickets
{
    public class InternalNoteDto
    {
        public int NoteId { get; set; }

        public int TicketId { get; set; }

        public int AgentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}