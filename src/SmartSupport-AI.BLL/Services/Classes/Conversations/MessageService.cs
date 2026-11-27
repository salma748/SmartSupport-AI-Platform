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

        public async Task<MessageDto?> SaveAIMessageAsync(int conversationId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Message content cannot be empty.");
            }

            var conversation = await _conversationRepository.GetByIdAsync(conversationId);

            if (conversation == null)
            {
                return null;
            }

            if (conversation.Status != "Active")
            {
                throw new InvalidOperationException("Cannot send messages to an inactive conversation.");
            }

            var messages = await _messageRepository.GetByConversationIdAsync(conversationId);

            var nextSequenceNo = messages.Count == 0
                ? 1
                : messages.Max(message => message.SequenceNo) + 1;

            var message = new Message
            {
                ConversationId = conversationId,
                SequenceNo = nextSequenceNo,
                SenderType = "AI",
                Content = content,
                SentAt = DateTime.UtcNow,
                CustomerId = null,
                AgentId = null
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

        public async Task<MessageDto?> SaveAgentMessageAsync(int conversationId,int agentId,string content)
        {
            if (conversationId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(conversationId));
            }

            if (agentId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(agentId));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Message content cannot be empty.");
            }

            var conversation = await _conversationRepository.GetByIdAsync(conversationId);

            if (conversation == null)
            {
                return null;
            }

            if (conversation.AgentId != agentId)
            {
                return null;
            }

            if (conversation.Status != "Active")
            {
                throw new InvalidOperationException(
                    "Cannot send messages to an inactive conversation.");
            }

            var messages = await _messageRepository.GetByConversationIdAsync(conversationId);

            var nextSequenceNo = messages.Count == 0
                ? 1
                : messages.Max(message => message.SequenceNo) + 1;

            var message = new Message
            {
                ConversationId = conversationId,
                SequenceNo = nextSequenceNo,
                SenderType = "Agent",
                Content = content,
                SentAt = DateTime.UtcNow,
                CustomerId = null,
                AgentId = agentId
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

        public async Task<List<MessageDto>?> GetRecentMessagesAsync(int conversationId,int customerId,int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count),"Count must be greater than zero.");

            var conversation = await _conversationRepository.GetByIdAsync(conversationId);

            if (conversation == null || conversation.CustomerId != customerId)
                return null;

            var messages = await _messageRepository
                .GetRecentByConversationIdAsync(conversationId, count);

            return messages.Select(m => new MessageDto
            {
                MessageId = m.MessageId,
                ConversationId = m.ConversationId,
                SequenceNo = m.SequenceNo,
                SenderType = m.SenderType,
                Content = m.Content,
                SentAt = m.SentAt
            }).ToList();
        }

        



    }
}
