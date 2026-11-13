namespace SmartSupport_AI.DAL.Entites
{
    public class SupportAgent
    {
        public int AgentId { get; set; }

        public int UserId { get; set; }

        public string Department { get; set; } = string.Empty;

        public string SkillLevel { get; set; } = string.Empty;

        public string Availability { get; set; } = string.Empty;

        public User User { get; set; } = null!;

        public ICollection<TicketAssignment> TicketAssignments { get; set; } = new List<TicketAssignment>();
    }
}
