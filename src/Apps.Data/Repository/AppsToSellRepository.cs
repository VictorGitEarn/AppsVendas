using Apps.Data.Base;
using Apps.Domain.Business;
using Apps.Data.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Apps.Data.Repository
{
    public class AppsToSellRepository : MongoDbRepository<AppsToSell>
    {
        public AppsToSellRepository(MongoDbContext dbContext) : base(dbContext) { }
    }
}
