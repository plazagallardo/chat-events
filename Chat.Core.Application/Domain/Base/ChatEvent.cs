using Chat.Core.Application.Domain.Enums;

namespace Chat.Core.Application.Domain.Base
{
    public abstract class ChatEvent : BaseEntity
    {
        public DateTime Date { get; set; }
        public string? UserName { get; set; }
        public EventType EventType { get; set; }
    }
}
