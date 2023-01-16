using Chat.Core.Application.Dtos;

namespace Chat.Core.Application.Domain
{
    public interface IChatEventService
    {
        Task<IEnumerable<ChatEventDto>> GetAllEventsActivityAsync(string timeGranularity);
        Task SendHighFive(DateTime date, string senderName, string recipientName);
        Task SendTextMessage(DateTime date, string senderName, string text);
    }
}
