using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class User
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string Role { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public Administrator? Administrator { get; set; }

        public Customer? Customer { get; set; }

        public SupportAgent? SupportAgent { get; set; }
    }
}
