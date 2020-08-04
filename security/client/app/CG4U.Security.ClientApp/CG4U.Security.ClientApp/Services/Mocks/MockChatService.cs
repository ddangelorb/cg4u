using System;
using System.Threading.Tasks;
using CG4U.Security.ClientApp.Models;
using CG4U.Security.ClientApp.Services.Interfaces;

namespace CG4U.Security.ClientApp.Services.Mocks
{
    public class MockChatService : IChatService
    {
        public MockChatService()
        {
        }

        public event EventHandler<ChatMessage> OnMessageReceived;

        public Task ConnectAsync()
        {
            return Task.CompletedTask;
        }

        public Task SendAsync(ChatMessage chatMessage)
        {
            return Task.CompletedTask;
        }
    }
}
