using System;

namespace CardProgram.Services
{
    public class VirtualCashCard
    {
        private static readonly object SyncLock = new object();
        protected ushort cardPin;
        protected decimal balance;

        protected internal void Initialise(ushort defaultPin, decimal initialBalance)
        {
            cardPin = defaultPin;
            balance = initialBalance;
        }

        protected internal virtual decimal Withdraw(decimal amount)
        {
            lock (SyncLock)
            {
                if (balance >= amount)
                {
                    balance -= amount;
                    return amount;
                }
            }

            throw new InvalidOperationException("Insufficient balance on card");
        }

        protected internal virtual bool TopUp(decimal amount)
        {
            try
            {
                lock (SyncLock)
                {
                    balance += amount;
                    return true;
                }
            }
            catch (OverflowException exception)
            {
                throw new InvalidOperationException($"Sorry amount could not be topped up. Card balance would exceed the allowable max limit of {decimal.MaxValue}");
            }
        }

        protected internal virtual bool ValidatePin(ushort suppliedCardPin)
        {
            return cardPin == suppliedCardPin;
        }

        protected internal virtual decimal GetBalance()
        {
            lock (SyncLock)
                return balance;
        }
    }
}