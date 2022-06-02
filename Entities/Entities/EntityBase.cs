using MongoDB.Bson;
using System;
using System.Runtime.Serialization;

namespace Entities.Entities
{
    [Serializable]
    public class EntityBase
    {
        [DataMember]
        public ObjectId Id { get; set; }
    }
}
