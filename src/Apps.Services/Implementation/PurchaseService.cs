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

        public PurchaseService(IBusControl bus, PurchaseRepository purchaseRepository)
        {
            _bus = bus;
            _purchaseRepository = purchaseRepository;
        }

        public Task<Purchase> AddProduct(string id)
        {
            throw new NotImplementedException();
        }

        public Task ClosePurchase(Purchase purchase, List<Payment> payment)
        {
            throw new NotImplementedException();
        }

        public async Task<Purchase> Create(Purchase purchase)
        {
            await _purchaseRepository.Insert(purchase);

            return purchase;
        }

        public Task DeleteProduct(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Purchase> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Purchase>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Purchase> GetById(string id)
        {
            return _purchaseRepository.FindById(new ObjectId(id));
        }

        public Task<Purchase> RemoveProduct(string id)
        {
            throw new NotImplementedException();
        }

        private async Task PublishPaymentMessage(List<Payment> payments)
        {
            foreach (var payment in payments)
            {
                await _bus.StartAsync();

                try
                {
                    var enpoint = await _bus.GetSendEndpoint(new Uri("exchange:apps_payments"));

                    await enpoint.Send(new PaymentMessage()
                    {
                        PaymentId = payment._id.ToString()
                    });

                    //atualizar payment para processando
                }
                catch (Exception)
                {
                    //atualizar status para erro interno
                }
                finally
                {
                    await _bus.StopAsync();
                }
            }
        }
    }
}
