using DataAbstraction;
using static DataAbstraction.CardSystemEnum;

namespace ChainOfResponsibilityBankCards.ConcreteHandlers
{
    public class HandlerToSystemVISA : AbstractHandler
    {
        public override CardEntityToPostAutoField Handle(CardSystem system)
        {
            if (system == CardSystem.Visa)
            {
                return new CardEntityToPostAutoField { System = CardSystem.Visa }; ;
            }
            else
            {
                return base.Handle(system);
            }
        }
    }
}
