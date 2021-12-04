using Apps.Domain.Business;

namespace Apps.APIRest.Models.ViewModels
{
    public class ProductModels
    {
        public ProductModels() { }

        public string _id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public ProductModels(Product product)
        {
            _id = product._id.ToString();
            Name = product.Name;
            Value = product.Value;
        }
    }
}
