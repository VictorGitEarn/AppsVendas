using Apps.Domain.Base;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Business
{
    public class Payment : EntityWithUserBase
    {
        public PaymentStatus Status { get; set; }
        
        public PaymentType Type { get; set; }

        public double Value { get; set; }

        public ObjectId CreditCardId { get; set; }
    }

    public enum PaymentStatus
    {
        Created = 0,
        Closed = 1,
        Refused = 2,
        InternalError = 3
    }

    public enum PaymentType
    {
        CreditCard = 1
    }
}
