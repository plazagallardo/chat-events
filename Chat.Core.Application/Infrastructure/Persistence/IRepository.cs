using Chat.Core.Application.Domain.Base;

namespace Chat.Core.Application.Infrastructure.Persistence
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync<Y>();
        Task AddAsync(TEntity entity);
    }
}
