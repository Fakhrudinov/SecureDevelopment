using DataAbstraction;
using static DataAbstraction.CardSystemEnum;

namespace AbstractFactoryBankCards.MIR
{
    internal class ConcreteProductCardDebetMIR : IAbstractCardMIR
    {
        public CardEntityToPostAutoField GetCardMIR()
        {
            return new CardEntityToPostAutoField { System = CardSystem.МИР };
        }
    }
}
