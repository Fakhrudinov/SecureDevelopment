using DataAbstraction;
using static DataAbstraction.CardSystemEnum;

namespace AbstractFactoryBankCards.VISA
{
    internal class ConcreteProductCardDebetVISA : IAbstractCardVISA
    {
        public CardEntityToPostAutoField GetCardVISA()
        {
            return new CardEntityToPostAutoField { System = CardSystem.Visa };
        }
    }
}
