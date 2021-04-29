using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Data.Entities
{
    public class BaseEntity
    {
        [BsonId]
        public Guid ID { get; set; }
    }
}
