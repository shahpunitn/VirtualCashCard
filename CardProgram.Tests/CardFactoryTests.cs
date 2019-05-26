using CardProgram.Services;
using CardProgram.Services.Interfaces;
using NUnit.Framework;

namespace CardProgram.Tests
{
    [TestFixture]
    public class CardFactoryTests
    {
        private ICardFactory<VirtualCashCard> _cardFactory;

        [SetUp]
        public void Initialise()
        {
            _cardFactory = new CardFactory<MockVirtualCashCard>();
        }

        [Test]
        public void Should_Create_Instance_Of_Card_When_Create_Card_Is_Called()
        {
            var card = _cardFactory.CreateCard(1234, 500);
            Assert.IsNotNull(card);
            Assert.IsInstanceOf<VirtualCashCard>(card);
        }

        [Test]
        public void Should_Initialise_Card_With_Default_Pin_And_Balance_When_Create_Card_Is_Called()
        {
            var card = _cardFactory.CreateCard(1234, 500) as MockVirtualCashCard;
            Assert.IsNotNull(card);
            Assert.AreEqual(1234, card.DefaultPin);
            Assert.AreEqual(500, card.Balance);
        }
    }
}
