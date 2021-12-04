using Apps.Domain.Business;

namespace Apps.APIRest.Models.ViewModels
{
    public class CartViewModels
    {
        public CartViewModels() { }

        public CartViewModels(Product product, int qtty)
        {
            Products = new List<ProductCartModels> { new ProductCartModels(product, qtty) };
            Qtty = qtty;
            TotalValue = product.Value * qtty;
        }

        public double TotalValue { get; set; }
        public int Qtty { get; set; }
        public List<ProductCartModels> Products { get; set; }

        public void AddProduct(Product product, int qtty)
        {
            var productInList = Products.Where(t => t._id == product._id.ToString()).FirstOrDefault();

            if (productInList is not null)
            {
                productInList.TotalValue += product.Value * qtty;

                productInList.Qtty += qtty;
            }
            else 
                Products.Add(new ProductCartModels(product, qtty));

            Qtty += qtty;

            TotalValue += product.Value * qtty;
        }
    }

    public class ProductCartModels : ProductModels
    {
        public ProductCartModels() { }

        public int Qtty { get; set; }

        public double TotalValue { get; set; }

        public ProductCartModels(Product product, int qtty) : base(product)
        {
            Qtty = qtty;

            TotalValue = product.Value * qtty;
        }
    }
}
