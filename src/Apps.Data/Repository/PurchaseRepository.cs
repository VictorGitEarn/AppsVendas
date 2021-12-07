using Apps.Data.Base;
using Apps.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Data.Repository
{
    public class PurchaseRepository : MongoDbRepository<Purchase>
    {
        public PurchaseRepository(MongoDbContext dbContext) : base(dbContext) { }


    }
}
