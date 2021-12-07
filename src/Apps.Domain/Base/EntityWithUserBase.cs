using MongoDB.Bson;

namespace Apps.Domain.Base
{
    public abstract class EntityWithUserBase : EntityBase
    {
        public ObjectId UserId { get; set; }
    }
}
