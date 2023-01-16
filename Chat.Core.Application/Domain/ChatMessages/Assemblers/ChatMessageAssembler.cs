using Chat.Core.Application.Domain.Base.Assemblers;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Dtos;
using Chat.Core.Application.Extensions;

namespace Chat.Core.Application.Domain.ChatMessages.Assemblers
{
    public class ChatMessageAssembler : ChatEventAssemblerBase<ChatMessage>
    {
        public override IEnumerable<ChatEventDto> AssembleChatEventDtosFrom(IEnumerable<ChatMessage> models, TimeGranularity timeGranularity)
        {

            if (timeGranularity == TimeGranularity.Seconds || timeGranularity == TimeGranularity.Minutes)
            {
                return models.Select(x =>
                {
                    return new ChatEventDto(x.Date.ToTimeViewString(timeGranularity), $"{x.UserName} {x.EventType.GetDescription()}: \"{x.Text}\"");
                });
            }

            return models
                .GroupBy(x => x.Date.ToTimeViewString(timeGranularity))
                .Select(y =>
                {
                    var totalComments = y.Count();
                    return new ChatEventDto(y.Key, $"{y.Count()} {(totalComments > 1 ? "comments" : "comment")}");
                });
        }
    }
}
