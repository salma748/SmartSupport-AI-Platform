namespace SmartSupport_AI.DAL.Entites
{
    public class CustomerFeedback
    {
        public int FeedbackId { get; set; }

        public int ConversationId { get; set; }

        public int? ResponseId { get; set; }

        public int CustomerId { get; set; }

        public int Rating { get; set; }

        public bool IsHelpful { get; set; }

        public string? Comment { get; set; }
    }
}
