using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public string PreferredLanguage { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
