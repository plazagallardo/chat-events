using Chat.Core.Application.Domain.Base;
using Chat.Core.Application.Domain.Enums;

namespace Chat.Core.Application.Domain
{
    public class ChatMessage : ChatEvent
    {
        public ChatMessage(DateTime date, string senderName, EventType eventType, string? text)
        {
            Date= date;
            Text = text;
            UserName = senderName;
            EventType = eventType;
        }

        public string? Text { get; set; }
    }
}
