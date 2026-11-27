using SmartSupport_AI.BLL.DTOs.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Interfaces.Conversations
{
    public interface IAttachmentService
    {
        Task<AttachmentDto?> UploadAttachmentAsync(
            int conversationId,
            int messageId,
            int customerId,
            Stream fileStream,
            string fileName,
            string contentType,
            long fileSize);

        Task<List<AttachmentDto>?> GetMessageAttachmentsAsync(int messageId,int customerId);
    }
}
