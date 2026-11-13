namespace SmartSupport_AI.DAL.Entites
{
    public class Intent
    {
        public int IntentId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<AIResponse> Responses { get; set; } = new List<AIResponse>();
    }
}
