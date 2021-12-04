using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Base
{
    public abstract class EntityBase
    {
        public ObjectId _id { get; set; }
    }
}
