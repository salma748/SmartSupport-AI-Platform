namespace SmartSupport_AI.BLL.DTOs.Users
{
    public class CustomerProfileDto
    {
        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string PreferredLanguage { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; }
    }
}
