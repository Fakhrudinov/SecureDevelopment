using DataAbstraction;
using static DataAbstraction.CardSystemEnum;

namespace AbstractFactoryBankCards.VISA
{
    internal class ConcreteProductCardCreditVISA : IAbstractCardVISA
    {
        public CardEntityToPostAutoField GetCardVISA()
        {
            return new CardEntityToPostAutoField { System = CardSystem.Visa };
        }
    }
}
