using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Branch : BaseEntity
    {
        public String name { get; set; }
        public IList<Guid> ChildrenIDs { get; set; }
        [BsonIgnore]
        public IList<Branch> Children { get; set; }
        public Guid parentID { get; set; }
        [BsonIgnore]
        public Branch parent { get; set; }
        public IList<Guid> positionIDs { get; set; }
        [BsonIgnore]
        public IList<Position> positions { get; set; }
    }
}