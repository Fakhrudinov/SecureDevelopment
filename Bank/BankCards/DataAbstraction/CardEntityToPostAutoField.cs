using static DataAbstraction.CardSystemEnum;
using static DataAbstraction.CardTypeEnum;

namespace DataAbstraction
{
    public class CardEntityToPostAutoField
    {
        public string HolderName { get; set; }
        public CardType Type { get; set; }
        public CardSystem System { get; set; }
    }
}
