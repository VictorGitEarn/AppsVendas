using Apps.Data.Repository;
using Apps.Domain.Business;
using Apps.Services.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository procuctRepository)
        {
            _productRepository = procuctRepository;
        }

        public async Task<List<Product>> FindAll()
        {
            var apps = await _productRepository.FindAll();
            
            if (apps.Count is not 0)
                return apps;

            apps = new Product().CreateSamples();

            await _productRepository.InsertMany(apps);

            return apps;
        }

        public async Task<Product> FindById(string id)
        {
            return await _productRepository.FindById(new ObjectId(id));
        }

        public async Task<List<Product>> FindByIds(List<ObjectId> ids)
        {
            return await _productRepository.FindByIds(ids);
        }
    }
}
