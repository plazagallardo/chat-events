using Chat.Core.Application.Domain.Base;
using Chat.Core.Application.Domain.Enums;

namespace Chat.Core.Application.Domain
{
    public class ChatHighFive : ChatEvent
    {
        public ChatHighFive(DateTime date, string senderName, string recipientName, EventType eventType)
        {
            Date = date;
            UserName = senderName;
            EventType = eventType;
            RecipientName = recipientName;
        }

        public string RecipientName { get; set; }
    }
}
