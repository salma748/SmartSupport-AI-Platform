namespace SmartSupport_AI.DAL.Entites
{
    public class Administrator
    {
        public int AdminId { get; set; }

        public int UserId { get; set; }

        public string AccessLevel { get; set; } = string.Empty;

        public User User { get; set; } = null!;
    }
}
