using System;
using CG4U.Security.ClientApp.Models;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.CustomCells
{
    public class ChatDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate textInDataTemplate;
        private readonly DataTemplate textOutDataTemplate;

        public ChatDataTemplateSelector()
        {
            this.textInDataTemplate = new DataTemplate(typeof(ChatIncomingViewCell));
            this.textOutDataTemplate = new DataTemplate(typeof(ChatOutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return !(item is ChatMessage messageVm) ? null : messageVm.IsIncoming ? this.textInDataTemplate : this.textOutDataTemplate;
        }
    }
}
