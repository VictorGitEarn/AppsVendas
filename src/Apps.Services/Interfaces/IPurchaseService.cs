using Apps.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Services.Interfaces
{
    public interface IPurchaseService
    {
        public Task<Purchase> Get(string id);

        public Task<List<Purchase>> GetAll();

        public Task<Purchase> AddProduct(string id);
        
        public Task<Purchase> RemoveProduct(string id);

        public Task DeleteProduct(string id);

        public Task<Purchase> Create(Purchase purchase);

        public Task ClosePurchase(Purchase purchase, List<Payment> payment);
    }
}
