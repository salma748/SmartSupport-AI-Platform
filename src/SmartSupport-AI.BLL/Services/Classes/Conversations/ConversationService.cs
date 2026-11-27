using SmartSupport_AI.BLL.DTOs.Conversations;
using SmartSupport_AI.BLL.Services.Interfaces.Conversations;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Classes.Conversations
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;

        public ConversationService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<List<ConversationDto>> GetCustomerConversationsAsync(int customerId)
        {
            var conversations = await _conversationRepository.GetByCustomerIdAsync(customerId);

            return conversations.Select(conversation => new ConversationDto
            {
                ConversationId = conversation.ConversationId,
                Status = conversation.Status,
                Summary = conversation.Summary,
                StartedAt = conversation.StartedAt,
                EndedAt = conversation.EndedAt
            }).ToList();
        }

        public async Task<ConversationDto?> GetConversationByIdAsync(int conversationId,int customerId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);

            if (conversation == null || conversation.CustomerId != customerId)
            {
                return null;
            }

            return new ConversationDto
            {
                ConversationId = conversation.ConversationId,
                Status = conversation.Status,
                Summary = conversation.Summary,
                StartedAt = conversation.StartedAt,
                EndedAt = conversation.EndedAt
            };
        }
    }
}
