using System;
using CardProgram.Services.Interfaces;

namespace CardProgram.Services
{
    public class CardServiceProvider : ICardServiceProvider
    {
        public decimal Withdraw(VirtualCashCard virtualCashCard, ushort cardPin, decimal amount)
        {
            ValidatePin(virtualCashCard, cardPin);
            ValidateAmount(amount);
            return virtualCashCard.Withdraw(amount);
        }

        public bool TopUp(VirtualCashCard virtualCashCard, ushort cardPin, decimal amount)
        {
            ValidatePin(virtualCashCard, cardPin);
            ValidateAmount(amount);
            return virtualCashCard.TopUp(amount);
        }

        public decimal GetBalance(VirtualCashCard virtualCashCard, ushort cardPin)
        {
            ValidatePin(virtualCashCard, cardPin);
            return virtualCashCard.GetBalance();
        }

        private static void ValidatePin(VirtualCashCard virtualCashCard, ushort cardPin)
        {
            if (!virtualCashCard.ValidatePin(cardPin))
                throw new InvalidOperationException("Invalid Card Pin");
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException($"Amount must be between 1 and {decimal.MaxValue}");
        }
    }
}