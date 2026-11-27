using SmartSupport_AI.BLL.DTOs.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Interfaces.Conversations
{
    public interface IConversationService
    {

        Task<ConversationDto> CreateConversationAsync(int customerId);

        Task<List<ConversationDto>> GetCustomerConversationsAsync(int customerId);

        Task<ConversationDto?> GetConversationByIdAsync(int conversationId, int customerId);

        Task<bool> UpdateConversationSummaryAsync(int conversationId,int customerId,string summary);

        Task<bool> CloseConversationAsync(int conversationId, int customerId);
    }
}