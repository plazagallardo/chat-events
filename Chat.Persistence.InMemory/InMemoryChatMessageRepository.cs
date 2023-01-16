using Chat.Core.Application.Domain;
using Chat.Core.Application.Infrastructure.Persistence;

namespace Chat.Persistence.InMemory
{
    public class InMemoryChatMessageRepository : IRepository<ChatMessage>
    {
        private readonly IList<ChatMessage> chatEvents = new List<ChatMessage>();

        public async Task<IEnumerable<ChatMessage>> GetAllAsync<Y>()
        {
            return await Task.FromResult(chatEvents.ToList());
        }

        public Task AddAsync(ChatMessage entity)
        {
            chatEvents.Add(entity);

            return Task.CompletedTask;
        }
    }
}