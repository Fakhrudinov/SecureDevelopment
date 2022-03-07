using AbstractFactoryBankCards.MasterCard;
using AbstractFactoryBankCards.MIR;
using AbstractFactoryBankCards.VISA;


namespace AbstractFactoryBankCards.DebetCards
{
    public class ConcreteFactoryCardsDebet : IAbstractFactory
    {
        public IAbstractCardVISA CreateCardVISA()
        {
            return new ConcreteProductCardDebetVISA();
        }

        public IAbstractCardMasterCard CreateCardMasterCard()
        {
            return new ConcreteProductCardDebetMasterCard();
        }
        public IAbstractCardMIR CreateCardMIR()
        {
            return new ConcreteProductCardDebetMIR();
        }
    }
}
