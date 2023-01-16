namespace Chat.Core.Application.Dtos
{
    public class ChatEventDto
    {
        public string? TimeScope { get; }
        public string? Message { get; }

        public ChatEventDto(string? timeScope, string? message)
        {
            TimeScope = timeScope;
            Message = message;
        }
    }
}
