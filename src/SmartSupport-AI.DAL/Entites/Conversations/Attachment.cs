using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class Attachment
    {
        public int AttachmentId { get; set; }

        public int? MessageId { get; set; }

        public int? TicketId { get; set; }

        public int UploadedBy { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
