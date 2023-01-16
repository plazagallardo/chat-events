using Chat.Core.Application.Domain;
using Chat.Core.Application.Infrastructure.Persistence;

namespace Chat.Persistence.InMemory
{
    public class InMemoryChatHighFiveMessageRepository : IRepository<ChatHighFive>
    {
        private readonly IList<ChatHighFive> chatEvents = new List<ChatHighFive>();

        public async Task<IEnumerable<ChatHighFive>> GetAllAsync<Y>()
        {
            return await Task.FromResult(chatEvents.ToList());
        }

        public Task AddAsync(ChatHighFive entity)
        {
            chatEvents.Add(entity);

            return Task.CompletedTask;
        }
    }
}