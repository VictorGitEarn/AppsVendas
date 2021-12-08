using Apps.Data.Repository;
using Apps.Domain.Business;
using Apps.MessageQueue.Message;
using Apps.Services.Interfaces;
using MassTransit;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Services.Implementation
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IBusControl _bus;
        private readonly PurchaseRepository _purchaseRepository;
        private readonly IPaymentService _paymentService;

        public PurchaseService(IBusControl bus, PurchaseRepository purchaseRepository, IPaymentService paymentService)
        {
            _bus = bus;
            _purchaseRepository = purchaseRepository;
            _paymentService = paymentService;
        }

        public async Task<bool> ClosePurchase(Purchase purchase, List<Payment> payments)
        {
            await _paymentService.InsertPayments(payments);

            purchase.Payments = payments.Select(t => t._id).ToList();

            purchase.Status = PurchaseStatus.Processing;

            _purchaseRepository.Replace(purchase);

            await PublishPaymentMessage(payments);

            return true;
        }

        public async Task<Purchase> Create(Purchase purchase, ObjectId userId)
        {
            purchase.UserId = userId;

            await _purchaseRepository.Insert(purchase);

            return purchase;
        }

        public async Task<bool> DeletePurchase(ObjectId id, ObjectId userId)
        {
            var purchase = await Get(id, userId);

            if (purchase == null || purchase.Status is not PurchaseStatus.Open)
                return false;

            _purchaseRepository.Delete(id);

            return true;
        }

        public async Task<Purchase> Get(ObjectId id, ObjectId userId) => await _purchaseRepository.GetById(id, userId);

        public async Task<List<Purchase>> GetAll(ObjectId userId) => await _purchaseRepository.GetAllByUser(userId);

        private async Task PublishPaymentMessage(List<Payment> payments)
        {
            foreach (var payment in payments)
            {
                await _bus.StartAsync();

                try
                {
                    var enpoint = await _bus.GetSendEndpoint(new Uri("exchange:apps_payments"));

                    await _paymentService.UpdatePaymentStatus(payment, PaymentStatus.Processing);

                    await enpoint.Send(new PaymentMessage()
                    {
                        PaymentId = payment._id.ToString()
                    });
                }
                catch (Exception)
                {
                    await _paymentService.UpdatePaymentStatus(payment, PaymentStatus.InternalError);
                }
                finally
                {
                    await _bus.StopAsync();
                }
            }
        }
    }
}
