using static DataAbstraction.CardSystemEnum;
using static DataAbstraction.CardTypeEnum;

namespace DataAbstraction
{
    public class CardEntityToPost
    {
        public string HolderName { get; set; }
        public string Number { get; set; }
        public string CVVCode { get; set; }
        public CardType Type { get; set; }
        public CardSystem System { get; set; }
        public bool IsBlocked { get; set; } = false;
    }
}
