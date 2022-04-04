using DataAbstraction;

namespace ChainOfResponsibilityBankCards
{
    public static class HandlersAccessor
    {
        public static CardEntityToPostAutoField GetObjectFromHandlers(AbstractHandler startHandler, CardSystemEnum.CardSystem system)
        {
            return startHandler.Handle(system);
            //var result = handler.Handle(system);

            //if (result != null)
            //{
            //    return result;
            //}
            //else
            //{
            //    return null;
            //}
        }
    }
}
