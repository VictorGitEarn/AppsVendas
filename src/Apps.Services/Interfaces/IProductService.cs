using Apps.Domain.Business;
using MongoDB.Bson;

namespace Apps.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> FindAll();

        Task<Product> FindById(string id);

        Task<List<Product>> FindByIds(List<ObjectId> ids);
    }
}
