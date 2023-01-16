using System.ComponentModel;

namespace Chat.Core.Application.Domain.Enums
{
    public enum EventType
    {
        [Description("enters the room")]
        EnterRoom,
        [Description("leaves")]
        LeaveRoom,
        [Description("comments")]
        SendMessage,
        [Description("high-fives")]
        SendHiFive
    }
}
