using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CardProgram.Services;
using CardProgram.Services.Interfaces;
using NUnit.Framework;

namespace CardProgram.Tests
{
    [TestFixture]
    public class CardServiceProviderTests
    {
        private ICardServiceProvider _cardServiceProvider;
        private ICardFactory<VirtualCashCard> _cardFactory;
        private VirtualCashCard _virtualCashCard;

        [SetUp]
        public void Initialize()
        {
            _cardFactory = new CardFactory<VirtualCashCard>();
            _cardServiceProvider = new CardServiceProvider();
            _virtualCashCard = _cardFactory.CreateCard(1234, 500);
        }

        [Test]
        public void Can_Withdraw_Money_When_Valid_Pin_Is_Supplied()
        {
            var amount = _cardServiceProvider.Withdraw(_virtualCashCard, 1234, 100);
            Assert.AreEqual(100, amount);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid Card Pin")]
        public void Throws_Invalid_Pin_Exception_When_Invalid_Pin_Is_Supplied_To_Withdraw()
        {
            _cardServiceProvider.Withdraw(_virtualCashCard, 5678, 100);
        }

        [Test]
        public void Adjust_Balance_When_Withdrawal_Is_Successful()
        {
            _cardServiceProvider.Withdraw(_virtualCashCard, 1234, 100);
            Assert.AreEqual(400, _cardServiceProvider.GetBalance(_virtualCashCard, 1234));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Insufficient balance on card")]
        public void Throws_Insufficient_Balance_Exception_When_Not_Enough_Balance_To_Withdraw()
        {
            _cardServiceProvider.Withdraw(_virtualCashCard, 1234, 600);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid Card Pin")]
        public void Throws_Invalid_Pin_Exception_When_Invalid_Pin_Is_Supplied_To_Get_Balance()
        {
            _cardServiceProvider.GetBalance(_virtualCashCard, 5678);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid Card Pin")]
        public void Throws_Invalid_Pin_Exception_When_Invalid_Pin_Is_Supplied_To_Topup()
        {
            _cardServiceProvider.TopUp(_virtualCashCard, 5678, 100);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throws_Invalid_Operation_Exception_When_Negative_Amount_Is_Supplied()
        {
            _cardServiceProvider.TopUp(_virtualCashCard, 1234, 0);
        }

        [Test]
        public void Adjust_Balance_When_Card_Is_Topped_Up()
        {
            _cardServiceProvider.TopUp(_virtualCashCard, 1234, 100);
            Assert.AreEqual(600, _cardServiceProvider.GetBalance(_virtualCashCard, 1234));
        }

        [Test]
        public void Can_Topup_When_Valid_Pin_Is_Supplied()
        {
            var toppedUp = _cardServiceProvider.TopUp(_virtualCashCard, 1234, 100);
            Assert.IsTrue(toppedUp);
        }

        [Test]
        public void Can_Withdraw_Cash_From_Multiple_Places_At_The_Same_Time()
        {
            Parallel.For(1, 4, i =>
            {
                _cardServiceProvider.Withdraw(_virtualCashCard, 1234, i * 50);
            });

            Assert.AreEqual(200, _cardServiceProvider.GetBalance(_virtualCashCard, 1234));
        }

        [Test]
        public void Can_Withdraw_Cash_From_Multiple_Places_At_Same_Time_Only_Until_Sufficient_Balance()
        {
            var taskResults = new ConcurrentQueue<Result>();
            Parallel.For(1, 6, i =>
            {
                var counter = i;
                var result = new Result {Key = counter};

                try
                {
                    var amount = _cardServiceProvider.Withdraw(_virtualCashCard, 1234, counter*50);
                    result.Amount = amount;
                    taskResults.Enqueue(result);

                }
                catch (Exception exception)
                {
                    result.ErrorMessage = exception.Message;
                    taskResults.Enqueue(result);
                }
            });

            var successfulTasks = taskResults.Where(r => string.IsNullOrWhiteSpace(r.ErrorMessage)).ToList();
            var failedTasks = taskResults.Where(r => !string.IsNullOrWhiteSpace(r.ErrorMessage)).ToList();
            Assert.AreEqual(4, successfulTasks.Count);
            Assert.AreEqual(1, failedTasks.Count);
            Assert.IsTrue(failedTasks.All(t => t.ErrorMessage.Equals("Insufficient balance on card")));
        }
    }

    class Result
    {
        public int Key { get; set; }
        public string ErrorMessage { get; set; }
        public decimal Amount { get; set; }
    }
}
