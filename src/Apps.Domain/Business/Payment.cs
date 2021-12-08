using Apps.Domain.Base;
using Apps.Domain.Observer;
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
        Processing = 1,
        Closed = 2,
        Refused = 3,
        InternalError = 4
    }

    public enum PaymentType
    {
        CreditCard = 1
    }
}
