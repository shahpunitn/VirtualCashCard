namespace CardProgram.Services.Interfaces
{
    public interface ICardFactory<out T> 
    {
        T CreateCard(ushort defaultPin, decimal initialBalance);
    }
}