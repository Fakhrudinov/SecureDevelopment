using AbstractFactoryBankCards.MasterCard;
using AbstractFactoryBankCards.MIR;
using AbstractFactoryBankCards.VISA;


namespace AbstractFactoryBankCards.CreditCards
{
    public class ConcreteFactoryCardsCredit : IAbstractFactory
    {
        public IAbstractCardVISA CreateCardVISA()
        {
            return new ConcreteProductCardCreditVISA();
        }

        public IAbstractCardMasterCard CreateCardMasterCard()
        {
            return new ConcreteProductCardCreditMasterCard();
        }

        public IAbstractCardMIR CreateCardMIR()
        {
            return new ConcreteProductCardCreditMIR();
        }

    }
}
