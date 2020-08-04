using System;
using System.Threading.Tasks;
using CG4U.Security.ClientApp.Models;

namespace CG4U.Security.ClientApp.Services.Interfaces
{
    public interface IChatService
    {
        Task ConnectAsync();
        Task SendAsync(ChatMessage chatMessage);
        event EventHandler<ChatMessage> OnMessageReceived;
    }
}
