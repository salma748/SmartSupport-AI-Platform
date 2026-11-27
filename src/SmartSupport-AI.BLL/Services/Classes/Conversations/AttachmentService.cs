using SmartSupport_AI.BLL.DTOs.Conversations;
using SmartSupport_AI.BLL.Services.Interfaces.Conversations;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Classes.Conversations
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IFileStorageService _fileStorageService;

        public AttachmentService(
            IAttachmentRepository attachmentRepository,
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IGenericRepository<Customer> customerRepository,
            IFileStorageService fileStorageService)
        {
            _attachmentRepository = attachmentRepository;
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _customerRepository = customerRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<AttachmentDto?> UploadAttachmentAsync(
                int conversationId,
                int messageId,
                int customerId,
                Stream fileStream,
                string fileName,
                string contentType,
                long fileSize)
        {
            if (fileStream == null || !fileStream.CanRead)
                throw new ArgumentException("Invalid file stream.");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be empty.");

            if (fileSize <= 0)
                throw new ArgumentException("File cannot be empty.");

            const long maxFileSize = 10 * 1024 * 1024;

            if (fileSize > maxFileSize)
                throw new ArgumentException("File size exceeds the 10 MB limit.");

            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            var allowedExtensions = new[] { ".pdf", ".png", ".jpg", ".jpeg" };

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("File type is not supported.");

            var conversation = await _conversationRepository
                .GetByIdAsync(conversationId);

            if (conversation == null || conversation.CustomerId != customerId)
                return null;

            var message = await _messageRepository.GetByIdAsync(messageId);

            if (message == null || message.ConversationId != conversationId)
                return null;


            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
                return null;

            var filePath = await _fileStorageService.SaveFileAsync(
                fileStream,
                fileName,
                contentType);

            try
            {

                var attachment = new Attachment
                {
                    MessageId = messageId,
                    TicketId = null,
                    UploadedBy = customer.UserId,
                    FileName = Path.GetFileName(fileName),
                    FileType = contentType,
                    FilePath = filePath,
                    FileSize = fileSize,
                    UploadedAt = DateTime.UtcNow
                };


                _attachmentRepository.Add(attachment);

                await _attachmentRepository.SaveChangesAsync();

                return new AttachmentDto
                {
                    AttachmentId = attachment.AttachmentId,
                    MessageId = messageId,
                    FileName = attachment.FileName,
                    FileType = attachment.FileType,
                    FileSize = attachment.FileSize,
                    UploadedAt = attachment.UploadedAt
                };
            }
            catch
            {
                // Remove the physical file if saving metadata fails.
                await _fileStorageService.DeleteFileAsync(filePath);

                throw;
            }
        }

        public async Task<List<AttachmentDto>?> GetMessageAttachmentsAsync(int messageId,int customerId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);

            if (message == null)
                return null;

            var conversation = await _conversationRepository.GetByIdAsync(message.ConversationId);

            if (conversation == null || conversation.CustomerId != customerId)
            {
                return null;
            }

            var attachments = await _attachmentRepository
                .GetByMessageIdAsync(messageId);

            return attachments.Select(a => new AttachmentDto
            {
                AttachmentId = a.AttachmentId,
                MessageId = a.MessageId!.Value,
                FileName = a.FileName,
                FileType = a.FileType,
                FileSize = a.FileSize,
                UploadedAt = a.UploadedAt
            }).ToList();
        }
    }
}
