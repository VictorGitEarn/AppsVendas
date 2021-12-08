using Apps.Data.Repository;
using Apps.Domain.Business;
using Apps.Domain.Observer;
using Apps.Helpers;
using Apps.MessageQueue.Message;
using Apps.Services.Interfaces;
using Apps.Services.Observer;
using MassTransit;
using MongoDB.Bson;
using System;

namespace Apps.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly CreditCardRepository _creditCardRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly IEncrypt_Decrypt _encrypt_Decrypt;
        private readonly List<IPaymentObserver> _observers;

        public PaymentService(CreditCardRepository creditCardRepository, PaymentRepository paymentRepository,
                              IPaymentObserverService paymentObserverService, IEncrypt_Decrypt encrypt_Decrypt)
        {
            _creditCardRepository = creditCardRepository;
            _paymentRepository = paymentRepository;
            _encrypt_Decrypt = encrypt_Decrypt;
            _observers = new List<IPaymentObserver>()
            {
                paymentObserverService
            };
        }

        public async Task ConsumeMessage(PaymentMessage message)
        {
            var payment = await _paymentRepository.FindById(new ObjectId(message.PaymentId));

            var creditCard = await _creditCardRepository.FindById(payment.CreditCardId);

            DecryptCrediCardData(creditCard);

            //Request externa com o processamento do pagamento com o cartão.

            if (creditCard.SaveCreditCardInfo is false)
                RemoveCreditCardData(creditCard);

            await UpdatePaymentStatus(payment, PaymentStatus.Closed);
        }

        private void NotifyPayment(Payment payment)
        {
            if (payment.Status == PaymentStatus.Created)
                return;

            foreach (var observer in _observers)
            {
                observer.NotifyPayment(payment);
            }
        }

        public async Task UpdatePaymentStatus(Payment payment, PaymentStatus status)
        {
            payment.Status = status;

            await _paymentRepository.UpdateStatus(payment);

            NotifyPayment(payment);
        }

        public async Task<List<CreditCard>> GetAllCards(ObjectId userId)
        {
            var cards = await _creditCardRepository.FindAllByUser(userId);

            cards.ForEach(card =>
            {
                if (card.SaveCreditCardInfo)
                    DecryptCrediCardData(card);
            });

            return cards;
        }

        public async Task<CreditCard> GetCreditCard(ObjectId id, ObjectId userId)
        {
            var card = await _creditCardRepository.FindById(id);

            if (card.UserId != userId)
                return null;

            if (card.SaveCreditCardInfo)
                DecryptCrediCardData(card);

            return card;
        }

        public async Task<CreditCard> ProcessCreditCard(CreditCard creditCard)
        {
            EncryptCreditCardData(creditCard);

            var card = await _creditCardRepository.FindCreditCardByNumber(creditCard.CreditCardNumber);

            if (card is null)
            {
                await _creditCardRepository.Insert(creditCard);
                return creditCard;
            }
            else
                return card;
        }

        private void EncryptCreditCardData(CreditCard creditCard)
        {
            creditCard.CreditCardNumber = _encrypt_Decrypt.EncryptString(creditCard.CreditCardNumber);
            creditCard.CVV = _encrypt_Decrypt.EncryptString(creditCard.CVV);
            creditCard.ExpireDate = _encrypt_Decrypt.EncryptString(creditCard.ExpireDate);
        }

        public async Task InsertPayments(List<Payment> payments)
        {
            await _paymentRepository.InsertMany(payments);
        }

        private void DecryptCrediCardData(CreditCard creditCard)
        {
            creditCard.CreditCardNumber = _encrypt_Decrypt.DecryptString(creditCard.CreditCardNumber);
            creditCard.CVV = _encrypt_Decrypt.DecryptString(creditCard.CVV);
            creditCard.ExpireDate = _encrypt_Decrypt.DecryptString(creditCard.ExpireDate);
        }

        private void RemoveCreditCardData(CreditCard creditCard)
        {
            creditCard.CVV = "***";
            creditCard.ExpireDate = "**/**";
            creditCard.FullName = "****** ****** *****";
            creditCard.CreditCardNumber = string.Concat("****-*****-****-", creditCard.CreditCardNumber.AsSpan(creditCard.CreditCardNumber.Length - 4));

            _creditCardRepository.Replace(creditCard);
        }

        public async Task<List<Payment>> FindByIds(List<ObjectId> ids)
        {
            return await _paymentRepository.FindByIds(ids);
        }

        public async Task<List<CreditCard>> FindCardsByIds(List<ObjectId> ids)
        {
            var cards = await _creditCardRepository.FindByIds(ids);

            cards.ForEach(card =>
            {
                if (card.SaveCreditCardInfo)
                    DecryptCrediCardData(card);
            });

            return cards;
        }
    }
}
