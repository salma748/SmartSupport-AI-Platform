using SmartSupport_AI.BLL.DTOs.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Interfaces.Conversations
{
    public interface IMessageService
    {
        Task<List<MessageDto>?> GetConversationMessagesAsync(int conversationId,int customerId);

        Task<MessageDto?> SendMessageAsync(int customerId, SendMessageDto messageDto); //for user

        Task<MessageDto?> SaveAIMessageAsync(int conversationId, string content);

        Task<MessageDto?> SaveAgentMessageAsync(int conversationId, int agentId, string content);

        Task<List<MessageDto>?> GetRecentMessagesAsync(int conversationId, int customerId, int count);
    }
}
