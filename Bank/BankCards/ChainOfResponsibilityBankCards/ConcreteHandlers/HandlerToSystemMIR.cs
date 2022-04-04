using DataAbstraction;
using static DataAbstraction.CardSystemEnum;

namespace ChainOfResponsibilityBankCards.ConcreteHandlers
{
    public class HandlerToSystemMIR : AbstractHandler
    {
        public override CardEntityToPostAutoField Handle(CardSystem system)
        {
            if (system == CardSystem.МИР)
            {
                return new CardEntityToPostAutoField { System = CardSystem.МИР };
            }
            else
            {
                return base.Handle(system);
            }
        }
    }
}
