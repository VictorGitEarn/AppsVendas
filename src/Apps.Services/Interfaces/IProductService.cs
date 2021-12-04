using Apps.Domain.Business;

namespace Apps.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> FindAll();
    }
}
