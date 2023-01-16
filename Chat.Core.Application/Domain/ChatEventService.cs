using Chat.Core.Application.Domain.Base.Assemblers;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Dtos;
using Chat.Core.Application.Infrastructure.Persistence;

namespace Chat.Core.Application.Domain
{
    public class ChatEventService : IChatEventService
    {
        private readonly IRepository<AccessRoomEvent> _accessEventRepository;
        private readonly IRepository<ChatMessage> _chatMessageRepository;
        private readonly IRepository<ChatHighFive> _chatHighRepository;
        private readonly IChatEventAssembler<AccessRoomEvent> _accessEventAssembler;
        private readonly IChatEventAssembler<ChatMessage> _chatMessageAssembler;
        private readonly IChatEventAssembler<ChatHighFive> _chatHighFiveAssembler;

        public ChatEventService(
            IRepository<AccessRoomEvent> accessEventRepository,
            IRepository<ChatMessage> chatMessageRepository,
            IRepository<ChatHighFive> chatHighRepository,
            IChatEventAssembler<AccessRoomEvent> accessEventAssembler,
            IChatEventAssembler<ChatMessage> chatMessageAssembler,
            IChatEventAssembler<ChatHighFive> chatHighFiveAssembler
            )
        {
            _accessEventRepository = accessEventRepository;
            _chatMessageRepository = chatMessageRepository;
            _chatHighRepository = chatHighRepository;
            _chatMessageAssembler = chatMessageAssembler;
            _chatHighFiveAssembler = chatHighFiveAssembler;
            _accessEventAssembler = accessEventAssembler;
        }

        public async Task<IEnumerable<ChatEventDto>> GetAllEventsActivityAsync(string timeGranularityFilter)
        {
            var timeGranularityParsed = Enum.TryParse<TimeGranularity>(timeGranularityFilter, out var timeGranularityType);

            if (!timeGranularityParsed)
            {
                throw new ArgumentException(nameof(timeGranularityFilter));
            }

            var finalDtos = new List<ChatEventDto>();

            var accessEvents = await _accessEventRepository.GetAllAsync<AccessRoomEvent>();
            if (accessEvents.Any())
            {
                AddAccessEventRoomDtos(accessEvents, finalDtos, timeGranularityType, EventType.EnterRoom);
                AddAccessEventRoomDtos(accessEvents, finalDtos, timeGranularityType, EventType.LeaveRoom);
            }

            var chatMessageEvents = await _chatMessageRepository.GetAllAsync<ChatMessage>();
            if (chatMessageEvents.Any())
            {
                var messageUserDtos = _chatMessageAssembler.AssembleChatEventDtosFrom(chatMessageEvents, timeGranularityType);
                finalDtos.AddRange(messageUserDtos);

            }

            var chatHighFiveEvents = await _chatHighRepository.GetAllAsync<ChatHighFive>();
            if (chatHighFiveEvents.Any())
            {
                var highFiveDtos = _chatHighFiveAssembler.AssembleChatEventDtosFrom(chatHighFiveEvents, timeGranularityType);
                finalDtos.AddRange(highFiveDtos);
            }

            return finalDtos.OrderBy(x => x.TimeScope);
        }

        private void AddAccessEventRoomDtos(IEnumerable<AccessRoomEvent> accessEvents, List<ChatEventDto> finalDtos, TimeGranularity timeGranularity, EventType eventType)
        {
            var accessRoomEvents = accessEvents.Where(x => x.EventType == eventType);
            if (!accessRoomEvents.Any())
            {
                return;
            }

            var chatEventDtos = _accessEventAssembler.AssembleChatEventDtosFrom(accessRoomEvents, timeGranularity);
            finalDtos.AddRange(chatEventDtos);
        }

        public async Task SendHighFive(DateTime date, string senderName, string recipientName)
        {
            if (string.IsNullOrEmpty(senderName))
            {
                throw new ArgumentNullException(nameof(senderName));
            }
            if (string.IsNullOrEmpty(recipientName))
            {
                throw new ArgumentNullException(nameof(recipientName));
            }

            await _chatHighRepository.AddAsync(new ChatHighFive(date, senderName, recipientName, EventType.SendHiFive));
        }

        public async Task SendTextMessage(DateTime date, string senderName, string text)
        {
            if (string.IsNullOrEmpty(senderName))
            {
                throw new ArgumentNullException(nameof(senderName));
            }

            await _chatMessageRepository.AddAsync(new ChatMessage(date, senderName, EventType.SendMessage, text));
        }
    }
}
