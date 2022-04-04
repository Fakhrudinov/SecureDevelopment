namespace AbstractFactoryBankCards
{
    public interface IAbstractFactory
    {
        IAbstractCardVISA CreateCardVISA();
        IAbstractCardMasterCard CreateCardMasterCard();
        IAbstractCardMIR CreateCardMIR();
    }
}