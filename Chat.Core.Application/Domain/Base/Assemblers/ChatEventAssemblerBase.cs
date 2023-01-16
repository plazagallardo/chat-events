using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Dtos;
using Chat.Core.Application.Extensions;

namespace Chat.Core.Application.Domain.Base.Assemblers
{
    public abstract class ChatEventAssemblerBase<T> : IChatEventAssembler<T> where T : ChatEvent
    {
        public abstract IEnumerable<ChatEventDto> AssembleChatEventDtosFrom(IEnumerable<T> models, TimeGranularity timeGranularity);

        protected IEnumerable<ChatEventDto> GetDefaultDtos(IEnumerable<ChatEvent> models, TimeGranularity timeGranularity)
        {
            return models.Select(x =>
            {
                return new ChatEventDto(x.Date.ToTimeViewString(timeGranularity), $"{x.UserName} {x.EventType.GetDescription()}");
            });
        }
    }

    public interface IChatEventAssembler<T> where T : ChatEvent
    {
        IEnumerable<ChatEventDto> AssembleChatEventDtosFrom(IEnumerable<T> models, TimeGranularity timeGranularity);
    }
}