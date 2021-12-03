using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Business.Interfaces
{
    public interface IUser
    {
        ObjectId GetUserId();
        string GetUserEmail();
        bool IsAuthenticated();
    }
}
