using Apps.Domain.Business;

namespace Apps.Services.Interfaces
{
    public interface IAppsToSellService
    {
        Task<List<AppsToSell>> FindAll();
    }
}
