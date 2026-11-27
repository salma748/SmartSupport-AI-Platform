using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.DTOs.Conversations
{
    public class AttachmentDto
    {
        public int AttachmentId { get; set; }
        public int MessageId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
