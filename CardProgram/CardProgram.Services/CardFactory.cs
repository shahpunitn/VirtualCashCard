using CardProgram.Services.Interfaces;

namespace CardProgram.Services
{
    public class CardFactory<T> : ICardFactory<T> where T : VirtualCashCard, new()
    {
        public T CreateCard(ushort defaultPin, decimal initialBalance)
        {
            var card = new T();
            card.Initialise(defaultPin, initialBalance);
            return card;
        }
    }
}