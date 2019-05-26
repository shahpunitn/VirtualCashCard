using System;
using CardProgram.Services;
using CardProgram.Services.Interfaces;

namespace CardProgram.Tests
{
    public class MockVirtualCashCard : VirtualCashCard
    {
        public ushort DefaultPin => cardPin;
        public decimal Balance => balance;
    }
}