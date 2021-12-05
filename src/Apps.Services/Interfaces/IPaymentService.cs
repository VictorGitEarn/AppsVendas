using Apps.Domain.Business;

namespace Apps.Services.Interfaces
{
    public interface IPaymentService
    {
        Task ConsumeMessage(Payment message);
    }
}
