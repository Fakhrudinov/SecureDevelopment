using DataAbstraction;
using static DataAbstraction.CardSystemEnum;

namespace ChainOfResponsibilityBankCards.ConcreteHandlers
{
    public class HandlerToSystemMasterCard : AbstractHandler
    {
        public override CardEntityToPostAutoField Handle(CardSystem system)
        {
            if (system == CardSystem.MasterCard)
            {
                return new CardEntityToPostAutoField { System = CardSystem.MasterCard };
            }
            else
            {
                return base.Handle(system);
            }
        }
    }
}
