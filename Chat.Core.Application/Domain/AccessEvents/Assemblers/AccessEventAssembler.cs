using Chat.Core.Application.Domain.Base.Assemblers;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Dtos;
using Chat.Core.Application.Extensions;

namespace Chat.Core.Application.Domain.AccessEvents.Assemblers
{
    public class AccessEventAssembler : ChatEventAssemblerBase<AccessRoomEvent>
    {
        public override IEnumerable<ChatEventDto> AssembleChatEventDtosFrom(IEnumerable<AccessRoomEvent> models, TimeGranularity timeGranularity)
        {
            if (models == null || !models.Any())
            {
                return Enumerable.Empty<ChatEventDto>();
            }

            if (timeGranularity == TimeGranularity.Seconds || timeGranularity == TimeGranularity.Minutes)
            {
                return GetDefaultDtos(models, timeGranularity);
            }

            return models
                .GroupBy(x => x.Date.ToTimeViewString(timeGranularity))
                .Select(y =>
                {
                    var totalPeople = y.Count();
                    var actionByEventType = y.Any(x => x.EventType == EventType.EnterRoom) ? "entered" : "left";
                    return new ChatEventDto(y.Key, $"{y.Count()} {(totalPeople > 1 ? "people" : "person")} {actionByEventType}");
                });
        }
    }
}
