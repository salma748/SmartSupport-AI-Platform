namespace SmartSupport_AI.BLL.DTOs.Tickets
{
    public class TicketAssignmentDto
    {
        public int AssignmentId { get; set; }

        public int TicketId { get; set; }

        public int AgentId { get; set; }

        public DateTime AssignedAt { get; set; }

        public bool IsCurrent { get; set; }
    }
}