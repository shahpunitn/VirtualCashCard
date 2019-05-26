namespace CardProgram.Services.Interfaces
{
    public interface ICardServiceProvider
    {
        decimal Withdraw(VirtualCashCard virtualCashCard, ushort cardPin, decimal amount);
        bool TopUp(VirtualCashCard virtualCashCard, ushort cardPin, decimal amount);
        decimal GetBalance(VirtualCashCard virtualCashCard, ushort cardPin);
    }
}