namespace SmartSupport_AI.BLL.DTOs.Users
{
    public class SupportAgentProfileDto
    {
        public int AgentId { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string SkillLevel { get; set; } = string.Empty;

        public string Availability { get; set; } = string.Empty;
    }
}
