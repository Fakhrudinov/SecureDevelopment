using DataAbstraction;

namespace ChainOfResponsibilityBankCards
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;

            return handler;
        }

        public virtual CardEntityToPostAutoField Handle(CardSystemEnum.CardSystem system)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(system);
            }
            else
            {
                return null;
            }
        }
    }
}