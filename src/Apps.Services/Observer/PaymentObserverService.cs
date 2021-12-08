using Apps.Data.Repository;
using Apps.Domain.Business;
using Apps.Domain.Observer;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Services.Observer
{
    public class PaymentObserverService : IPaymentObserverService
    {
        private readonly PurchaseRepository _purchaseRepository;
        private readonly PaymentRepository _paymentRepository;

        public PaymentObserverService(PurchaseRepository purchaseRepository, PaymentRepository paymentRepository)
        {
            _purchaseRepository = purchaseRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task NotifyPayment(Payment payment)
        {
            var purchase = await _purchaseRepository.FindByPaymentId(new List<ObjectId>() { payment._id });

            var payments = await GetPaymentsFromPurchase(purchase);

            if (payments.Any(t => t.Status == PaymentStatus.Created))
                return;

            switch (payment.Status)
            {
                case PaymentStatus.Closed:
                    purchase.Status = PurchaseStatus.Closed;
                    break;
                case PaymentStatus.Refused:
                    purchase.Status = PurchaseStatus.PaymentRefused;
                    break;
                case PaymentStatus.InternalError:
                    purchase.Status = PurchaseStatus.Error;
                    break;
                default:
                    break;
            }

            await _purchaseRepository.UpdateStatus(purchase);
        }

        private async Task<List<Payment>> GetPaymentsFromPurchase(Purchase purchase) => await _paymentRepository.FindByIds(purchase.Payments);
    }
}
