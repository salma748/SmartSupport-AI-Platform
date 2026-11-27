using SmartSupport_AI.BLL.DTOs.Conversations;
using SmartSupport_AI.BLL.Services.Interfaces.Conversations;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Classes.Conversations
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IAIConfigurationRepository _aiConfigurationRepository;

        public ConversationService(IConversationRepository conversationRepository, IAIConfigurationRepository aiConfigurationRepository)
        {
            _conversationRepository = conversationRepository;
            _aiConfigurationRepository = aiConfigurationRepository;
        }

        public async Task<ConversationDto> CreateConversationAsync(int customerId)
        {
            var activeConfig = await _aiConfigurationRepository.GetActiveAsync();

            if (activeConfig == null)
            {
                throw new InvalidOperationException(
                    "No active AI configuration is available.");
            }

            var conversation = new Conversation
            {
                CustomerId = customerId,
                ConfigId = activeConfig.ConfigId,
                Status = "Active",
                StartedAt = DateTime.UtcNow
            };

            _conversationRepository.Add(conversation);

            await _conversationRepository.SaveChangesAsync();

            return new ConversationDto
            {
                ConversationId = conversation.ConversationId,
                Status = conversation.Status,
                Summary = conversation.Summary,
                StartedAt = conversation.StartedAt,
                EndedAt = conversation.EndedAt
            };
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

        public async Task<bool> UpdateConversationSummaryAsync(int conversationId,int customerId,string summary)
        {
            if (string.IsNullOrWhiteSpace(summary))
                throw new ArgumentException("Summary cannot be empty.",nameof(summary));

            var conversation = await _conversationRepository.GetByIdAsync(conversationId);


            if (conversation == null || conversation.CustomerId != customerId)
                return false;

            conversation.Summary = summary;

            _conversationRepository.Update(conversation);

            await _conversationRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CloseConversationAsync(int conversationId,int customerId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);

            if (conversation == null || conversation.CustomerId != customerId)
                return false;

            conversation.Status = "Closed";
            conversation.EndedAt = DateTime.UtcNow;

            _conversationRepository.Update(conversation);

            await _conversationRepository.SaveChangesAsync();

            return true;
        }
    }
}
