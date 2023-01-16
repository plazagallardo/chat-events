using Chat.Core.Application.Domain;
using Chat.Core.Application.Infrastructure.Persistence;

namespace Chat.Persistence.InMemory
{
    public class InMemoryChatAccessRoomEventMessageRepository : IRepository<AccessRoomEvent>
    {
        private readonly IList<AccessRoomEvent> chatEvents = new List<AccessRoomEvent>();

        public async Task<IEnumerable<AccessRoomEvent>> GetAllAsync<Y>()
        {
            return await Task.FromResult(chatEvents.ToList());
        }

        public Task AddAsync(AccessRoomEvent entity)
        {
            chatEvents.Add(entity);

            return Task.CompletedTask;
        }
    }
}