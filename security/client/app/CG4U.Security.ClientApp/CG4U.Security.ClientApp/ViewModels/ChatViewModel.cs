using System;
using System.Net;
using System.Threading.Tasks;
using CG4U.Security.ClientApp.Models;
using CG4U.Security.ClientApp.Services.Interfaces;
using MvvmHelpers;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private readonly IChatService _chatService;
        private string _nameOutGoingText;
        private string _outgoingText;
        private Command _sendCommand;
        private Command _locationCommand;

        public string OutGoingText
        {
            get { return _outgoingText; }
            set { SetProperty(ref _outgoingText, value); }
        }
        public Command SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new Command(async () => await ExecuteSendCommand()));
            }
        }
        public Command LocationCommand
        {
            get
            {
                return _locationCommand ?? (_locationCommand = new Command(async () => await ExecuteLocationCommand()));
            }
        }

        public ObservableRangeCollection<ChatMessage> Messages { get; }

        public ChatViewModel()
        {
            _nameOutGoingText = Application.Current.Properties["userName"].ToString();
            _outgoingText = string.Empty;
            _chatService = DependencyService.Get<IChatService>();
            Messages = new ObservableRangeCollection<ChatMessage>();
        }

        private async Task ExecuteSendCommand()
        {
            var webClient = new WebClient();
            var bImg = webClient.DownloadData("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQgjeox7Z3kq4LAkAGElqZH2C3Ce4rr2DmYMvDLND5-Ixnfbj1e");

            var message = new ChatMessage
            {
                Name = _nameOutGoingText,
                Text = OutGoingText,
                MessageDateTime = DateTime.Now,
                IsIncoming = true,
                ImageFile = bImg
            };
            Messages.Add(message);
            await _chatService?.SendAsync(message);
            OutGoingText = string.Empty;
        }

        private async Task ExecuteLocationCommand()
        {
            await Task.CompletedTask;
            /*
                //https://github.com/jamesmontemagno/app-monkeychat/blob/master/src/MonkeyChat/ViewModels/MainChatViewModel.cs
                try
                {
                    var local = await CrossGeolocator.Current.GetPositionAsync(10000);
                    var map = $"https://maps.googleapis.com/maps/api/staticmap?center={local.Latitude.ToString(CultureInfo.InvariantCulture)},{local.Longitude.ToString(CultureInfo.InvariantCulture)}&zoom=17&size=400x400&maptype=street&markers=color:red%7Clabel:%7C{local.Latitude.ToString(CultureInfo.InvariantCulture)},{local.Longitude.ToString(CultureInfo.InvariantCulture)}&key=";

                    var message = new Message
                    {
                        Text = "I am here",
                        AttachementUrl = map,
                        IsIncoming = false,
                        MessageDateTime = DateTime.Now
                    };

                    Messages.Add(message);
                    twilioMessenger?.SendMessage("attach:" + message.AttachementUrl);

                }
                catch (Exception ex)
                {

                }
             */
        }
    }
}
