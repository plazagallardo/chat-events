namespace Chat.Core.Application.Domain.Base
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}