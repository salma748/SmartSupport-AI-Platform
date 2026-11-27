using SmartSupport_AI.BLL.DTOs.Conversations;
using SmartSupport_AI.BLL.Services.Interfaces.Conversations;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Classes.Conversations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;

        public MessageService(IMessageRepository messageRepository,IConversationRepository conversationRepository)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
        }

        public async Task<List<MessageDto>?> GetConversationMessagesAsync(int conversationId,int customerId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);

            if (conversation == null || conversation.CustomerId != customerId)
            {
                return null;
            }

            var messages = await _messageRepository.GetByConversationIdAsync(conversationId);

            return messages.Select(message => new MessageDto
            {
                MessageId = message.MessageId,
                ConversationId = message.ConversationId,
                SequenceNo = message.SequenceNo,
                SenderType = message.SenderType,
                Content = message.Content,
                SentAt = message.SentAt
            }).ToList();
        }

        public async Task<MessageDto?> SendMessageAsync(int customerId,SendMessageDto messageDto)
        {
            if (string.IsNullOrWhiteSpace(messageDto.Content))
            {
                throw new ArgumentException("Message content cannot be empty.");
            }

            var conversation = await _conversationRepository.GetByIdAsync(messageDto.ConversationId);

            if (conversation == null || conversation.CustomerId != customerId)
            {
                return null;
            }

            if (conversation.Status != "Active")
            {
                throw new InvalidOperationException("Cannot send messages to an inactive conversation.");
            }

            var messages = await _messageRepository.GetByConversationIdAsync(messageDto.ConversationId);

            var nextSequenceNo = messages.Count == 0
                ? 1
                : messages.Max(message => message.SequenceNo) + 1;

            var message = new Message
            {
                ConversationId = messageDto.ConversationId,
                SequenceNo = nextSequenceNo,
                SenderType = "Customer",
                Content = messageDto.Content,
                SentAt = DateTime.UtcNow,
                CustomerId = customerId
            };

            _messageRepository.Add(message);

            await _messageRepository.SaveChangesAsync();

            return new MessageDto
            {
                MessageId = message.MessageId,
                ConversationId = message.ConversationId,
                SequenceNo = message.SequenceNo,
                SenderType = message.SenderType,
                Content = message.Content,
                SentAt = message.SentAt
            };
        }
    }
}
