using SmartSupport_AI.BLL.DTOs.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.Services.Interfaces.Conversations
{
    public interface IMessageService
    {
        Task<List<MessageDto>?> GetConversationMessagesAsync(int conversationId,int customerId);

        Task<MessageDto?> SendMessageAsync(int customerId, SendMessageDto messageDto);
    }
}
