using DataAbstraction;
using static DataAbstraction.CardSystemEnum;

namespace AbstractFactoryBankCards.MasterCard
{
    internal class ConcreteProductCardDebetMasterCard : IAbstractCardMasterCard
    {
        public CardEntityToPostAutoField GetCardMasterCard()
        {
            return new CardEntityToPostAutoField { System = CardSystem.MasterCard };
        }
    }
}
