using Chat.Core.Application.Domain.Base;
using Chat.Core.Application.Domain.Enums;

namespace Chat.Core.Application.Domain
{
    public class AccessRoomEvent : ChatEvent
    {
        public AccessRoomEvent(DateTime date, string senderName, EventType eventType)
        {
            Date = date;
            UserName = senderName;
            EventType = eventType;
        }
    }
}
