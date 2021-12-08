using Apps.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Domain.Observer
{
    public interface IPaymentObserver
    {
        Task NotifyPayment(Payment payment);
    }
}
