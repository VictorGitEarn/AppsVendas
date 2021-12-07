using Apps.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Business
{
    public class CreditCard : EntityWithUserBase
    {
        public bool SaveCreditCardInfo { get; set; }

        public string CreditCardNumber { get; set; }

        public string CVV { get; set; }

        public string ExpireDate { get; set; }

        public string FullName { get; set; }
    }
}
