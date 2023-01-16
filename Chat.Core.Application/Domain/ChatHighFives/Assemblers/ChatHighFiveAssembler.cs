using Chat.Core.Application.Domain.Base.Assemblers;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Dtos;
using Chat.Core.Application.Extensions;

namespace Chat.Core.Application.Domain.HighFives.Assemblers
{
    public class ChatHighFiveAssembler : ChatEventAssemblerBase<ChatHighFive>
    {
        public override IEnumerable<ChatEventDto> AssembleChatEventDtosFrom(IEnumerable<ChatHighFive> models, TimeGranularity timeGranularity)
        {
            if (timeGranularity == TimeGranularity.Seconds || timeGranularity == TimeGranularity.Minutes)
            {
                return models.Select(x =>
                {
                    return new ChatEventDto(x.Date.ToTimeViewString(timeGranularity), $"{x.UserName} {x.EventType.GetDescription()} {x.RecipientName}");
                });
            }

            return models
                .GroupBy(x => x.Date.ToTimeViewString(timeGranularity))
                .Select(y =>
                {
                    var totalSenderPeople = y.Select(x => x.UserName).Distinct().Count();
                    var totalRecipientPeople = y.Select(x => x.RecipientName).Distinct().Count();
                    return new ChatEventDto(y.Key, $"" +
                        $"{totalSenderPeople} {(totalSenderPeople > 1 ? "people" : "person")}" +
                        $" high-fived {totalRecipientPeople} other {(totalRecipientPeople > 1 ? "people" : "person")}");
                });
        }
    }
}
