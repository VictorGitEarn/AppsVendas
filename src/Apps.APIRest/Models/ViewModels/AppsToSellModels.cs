using Apps.Domain.Business;

namespace Apps.APIRest.Models.ViewModels
{
    public class AppsToSellModels
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public AppsToSellModels(AppsToSell appsToSell)
        {
            _id = appsToSell._id.ToString();
            Name = appsToSell.Name;
            Value = appsToSell.Value;
        }
    }
}
