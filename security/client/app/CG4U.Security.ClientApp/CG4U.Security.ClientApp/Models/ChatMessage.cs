using System;
using System.IO;
using MvvmHelpers;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.Models
{
    public class ChatMessage : ObservableObject
    {
        private string _name;
        private string _text;
        private DateTime _messageDateTime;
        private bool _isIncoming;
        private string _attachementUrl;
        private byte[] _imageFile;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        public DateTime MessageDateTime
        {
            get { return _messageDateTime; }
            set { SetProperty(ref _messageDateTime, value); }
        }
        public string TimeDisplay => MessageDateTime.ToLocalTime().ToString();
        public bool IsIncoming
        {
            get { return _isIncoming; }
            set { SetProperty(ref _isIncoming, value); }
        }
        public bool HasAttachement => !string.IsNullOrEmpty(_attachementUrl);
        public string AttachementUrl
        {
            get { return _attachementUrl; }
            set { SetProperty(ref _attachementUrl, value); }
        }
        public byte[] ImageFile
        {
            get { return _imageFile; }
            set { SetProperty(ref _imageFile, value); }
        }
        public ImageSource ImageFileSorce
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(ImageFile));
            }
        }
    }
}
