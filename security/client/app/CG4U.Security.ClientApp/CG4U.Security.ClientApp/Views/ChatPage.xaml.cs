using System;
using System.Collections.Generic;
using System.Net;
using CG4U.Security.ClientApp.Models;
using CG4U.Security.ClientApp.ViewModels;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.Views
{
    public partial class ChatPage : ContentPage
    {
        private ChatViewModel _viewModel;

        public ChatPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ChatViewModel();
           
            //https://github.com/juucustodio/Chat-Xamarin.Forms
            //https://github.com/L3xer/SignalR-Chat
            //https://github.com/jamesmontemagno/app-monkeychat
            var itens = new List<ChatMessage>()
            {
                new ChatMessage() { Name = "Suporte", Text="Olá Daniel! Por favor verifique seus alertas...", IsIncoming=false, MessageDateTime=DateTime.Now, ImageFile=GetImageMockAsync(1)},
                new ChatMessage() { Name = "Fabiana Barbosa", Text="Olá! Tudo bem?", IsIncoming=true, MessageDateTime=DateTime.Now, ImageFile=GetImageMockAsync(2)}
            };
            _viewModel.Messages.AddRange(itens);
            //MessageList.ItemsSource = itens;
            //MessagesListView.ItemsSource = itens;
            //????
            //https://github.com/jamesmontemagno/ImageCirclePlugin

            _viewModel.Messages.CollectionChanged += (sender, e) =>
            {
                var target = _viewModel.Messages[_viewModel.Messages.Count - 1];
                ChatListView.ScrollTo(target, ScrollToPosition.End, true);
            };
        }

        private byte[] GetImageMockAsync(int id)
        {
            var filePath = string.Empty;
            if (id == 1) filePath = "https://www.wifibr.com.br/wp-content/uploads/2017/11/suporte2.png";
            else if (id == 2) filePath = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQgjeox7Z3kq4LAkAGElqZH2C3Ce4rr2DmYMvDLND5-Ixnfbj1e";
            else filePath = "https://img.youtube.com/vi/Xc0d510zTA4/default.jpg";

            var webClient = new WebClient();
            return webClient.DownloadData(filePath);
        }

        private async void OnToolbarNewClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private void OnChatListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ChatListView.SelectedItem = null;
        }

        private void OnChatListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            ChatListView.SelectedItem = null;
        }
    }
}
