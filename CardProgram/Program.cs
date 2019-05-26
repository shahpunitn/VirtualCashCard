using System;
using CardProgram.Services;
using CardProgram.Services.Interfaces;
using Unity;

namespace CardProgram
{
    /*
        This is a client application that uses the card service provider 
        to either withdraw from or top up amount to the card.

        Assumptions:
         - A new Card can only be created via Card Factory 
         - Client can only perform operations on card via card service provider
         - Only Withdraw, Top-up and Balance operations are supported currently via service provider to keep the program simple
         - Pin is to be provided on each and every transaction i.e. either Withdraw or Top-up or Balance, and so it needs to be supplied every time 
    */
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var container = UnityBootstrapper.Initialize();
                var cardServiceProvider = container.Resolve<ICardServiceProvider>();
                var cardFactory = container.Resolve<ICardFactory<VirtualCashCard>>();
                var card = cardFactory.CreateCard(1234, 500);

                while (true)
                {
                    Console.WriteLine("Please select from the following options:");
                    Console.Write("Enter 1 to withdraw amount or 2 to Top-up or Q|q to quit: ");
                    var response = Console.ReadLine();

                    try
                    {
                        switch (response)
                        {
                            case "Q":
                            case "q":
                                Environment.Exit(0);
                                break;

                            case "1":
                                var withdrawAmount = cardServiceProvider.Withdraw(card, RequestPin(), RequestAmount());
                                Console.WriteLine($"{withdrawAmount:C} withdrawn successfully\n\n");
                                break;

                            case "2":
                                var pin = RequestPin();
                                var topUpAmount = RequestAmount();
                                cardServiceProvider.TopUp(card, pin, topUpAmount);
                                Console.WriteLine($"{topUpAmount:C} topped up successfully\n\n");
                                break;

                            default:
                                WriteError("Invalid option entered. Try again!\n\n");
                                break;
                        }
                    }
                    catch (InvalidOperationException exception) 
                    {
                        WriteError($"{exception.Message}\n\n");
                    }
                }
            }
            catch (Exception exception)
            {
                WriteError($"{exception.Message}\n\n");
            }

            Console.ReadLine();
        }

        private static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static decimal RequestAmount()
        {
            Console.Write("Enter amount: ");
            decimal amount;
            decimal.TryParse(Console.ReadLine(), out amount);
            return amount;
        }

        private static ushort RequestPin()
        {
            Console.Write("Enter card pin number: ");
            ushort pin;
            ushort.TryParse(Console.ReadLine(), out pin);

            return pin;
        }
    }
}
