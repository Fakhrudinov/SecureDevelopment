using DataAbstraction;

namespace ChainOfResponsibilityBankCards
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        CardEntityToPostAutoField Handle(CardSystemEnum.CardSystem system);
    }
}
