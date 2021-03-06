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
    public class Purchase : EntityWithUserBase
    {
        public string Number { get; set; }

        public double Value { get; set; }

        public List<PurchaseProductModel> Products { get; set; }

        public List<ObjectId> Payments { get; set; }

        public PurchaseStatus Status { get; set; }
    }

    public enum PurchaseStatus
    {
        Open = 0,
        Processing = 1,
        Closed = 2,
        PaymentRefused = 3,
        Error = 4,
    }

    public class PurchaseProductModel
    {
        public ObjectId ProductId { get; set; }

        public int Qtty { get; set; }

        public double TotalValue { get; set; }
    }
}
