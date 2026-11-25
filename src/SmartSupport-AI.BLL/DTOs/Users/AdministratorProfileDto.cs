namespace SmartSupport_AI.BLL.DTOs.Users
{
    public class AdministratorProfileDto
    {
        public int AdminId { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;
    }
}
