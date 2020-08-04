using System;
using System.Threading.Tasks;
using CG4U.Security.ClientApp.Models;
using CG4U.Security.ClientApp.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace CG4U.Security.ClientApp.Services
{
    public class ChatService : IChatService
    {
        private readonly HubConnection _connection;

        public ChatService()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://signalxtest.azurewebsites.net/chat")
                .Build();

            _connection.On("sendToAll", (string name, string text, DateTime messageDateTime, bool isTextIn) => OnMessageReceived(this, new ChatMessage
            {
                Name = name,
                Text = text,
                MessageDateTime = messageDateTime,
                IsIncoming = isTextIn
            }));
        }

        public event EventHandler<ChatMessage> OnMessageReceived;

        public async Task ConnectAsync()
        {
            await _connection.StartAsync();
        }

        public async Task SendAsync(ChatMessage chatMessage)
        {
            await _connection.InvokeAsync("sendToAll", chatMessage.Name, chatMessage.Text, chatMessage.MessageDateTime, chatMessage.IsIncoming);
        }
    }
}
