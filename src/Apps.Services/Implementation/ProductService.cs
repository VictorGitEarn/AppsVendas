using Apps.Data.Repository;
using Apps.Domain.Business;
using Apps.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _appsToSellRepository;

        public ProductService(ProductRepository appsToSellRepository)
        {
            _appsToSellRepository = appsToSellRepository;
        }

        public async Task<List<Product>> FindAll()
        {
            var apps = await _appsToSellRepository.FindAll();
            
            if (apps.Count is not 0)
                return apps;

            apps = new Product().CreateSamples();

            await _appsToSellRepository.InsertMany(apps);

            return apps;
        }
    }
}
