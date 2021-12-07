using Apps.Data.Repository;
using Apps.Domain.Business;
using Apps.Helpers;
using Apps.MessageQueue.Message;
using Apps.Services.Interfaces;
using MassTransit;
using MongoDB.Bson;

namespace Apps.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly CreditCardRepository _creditCardRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly IEncrypt_Decrypt _encrypt_Decrypt;

        public PaymentService(CreditCardRepository creditCardRepository, PaymentRepository paymentRepository, IEncrypt_Decrypt encrypt_Decrypt)
        {
            _creditCardRepository = creditCardRepository;
            _paymentRepository = paymentRepository;
            _encrypt_Decrypt = encrypt_Decrypt;
        }

        public async Task ConsumeMessage(PaymentMessage message)
        {
            var payment = await _paymentRepository.FindById(new ObjectId(message.PaymentId));



        }

        public async Task<List<CreditCard>> GetAllCards() => await _creditCardRepository.FindAll();

        public async Task<CreditCard> GetCreditCard(string id) => await _creditCardRepository.FindById(new ObjectId(id));

        public async Task<CreditCard> ProcessCreditCard(CreditCard creditCard)
        {
            creditCard.CreditCardNumber = _encrypt_Decrypt.EncryptString(creditCard.CreditCardNumber);

            var card = await _creditCardRepository.FindCreditCardByNumber(creditCard.CreditCardNumber);

            if (card is null)
            {
                await _creditCardRepository.Insert(creditCard);
                return creditCard;
            }
            else
                return card;
        }
    }
}
