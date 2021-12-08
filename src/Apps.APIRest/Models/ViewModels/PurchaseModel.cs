using Apps.Domain.Business;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Apps.APIRest.Models.ViewModels
{
    public class PurchaseModel
    {
        public PurchaseModel() { }

        public string _id { get; set; }

        public string Number { get; set; }

        public double TotalValue { get; set; }
        
        public int Qtty { get; set; }

        public List<ProductCartModels> Products { get; set; }

        public List<PaymentModel> Payments { get; set; }

        public string PurchaseStatus { get; set; }

        public PurchaseModel(Purchase purchase, List<Product> products, List<Payment> payments, List<CreditCard> creditCards)
        {
            _id = purchase._id.ToString();
            Number = purchase.Number;
            PurchaseStatus = purchase.Status.ToString();
            TotalValue = purchase.Value;
            Qtty = purchase.Products.Sum(t => t.Qtty);
            Products = purchase.Products.Select(t => new ProductCartModels(t, products.First(p => p._id == t.ProductId))).ToList();
            Payments = payments.Select(t => new PaymentModel(t, creditCards)).ToList();
        }
    }

    public class PaymentModel
    {
        public PaymentModel() { }

        public PaymentType PaymentType { get; set; }

        public string Status { get; set; }

        public CreditCardModel CreditCard { get; set; }

        public double Value { get; set; }

        public PaymentModel(Payment payment, List<CreditCard> creditCards)
        {
            PaymentType = payment.Type;
            Status = payment.Status.ToString();
            Value = payment.Value;
            CreditCard = new CreditCardModel(creditCards.First(t => t._id == payment.CreditCardId));
        }

        public Payment MapToPayment(ObjectId userId)
        {
            return new Payment()
            {
                UserId = userId,
                Type = PaymentType,
                Value = Value
            };
        }
    }


    public class CreditCardModel
    {
        public CreditCardModel() { }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool SaveCreditCardInfo { get; set; }

        [CreditCard(ErrorMessage = "Cartão de crédito inválido")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string CVV { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string ExpireDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string FullName { get; set; }

        public CreditCardModel(CreditCard creditCard)
        {
            SaveCreditCardInfo = creditCard.SaveCreditCardInfo;
            CreditCardNumber = creditCard.CreditCardNumber;
            CVV = creditCard.CVV;
            ExpireDate = creditCard.ExpireDate;
            FullName = creditCard.FullName;
        }

        public CreditCard MapToCreditCard(ObjectId userId)
        {
            return new CreditCard()
            {
                UserId = userId,
                SaveCreditCardInfo = SaveCreditCardInfo,
                CreditCardNumber = CreditCardNumber,
                FullName = FullName,
                CVV = CVV,
                ExpireDate = ExpireDate
            };
        }
    }
}
