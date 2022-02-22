using static DataAbstraction.CardSystemEnum;
using static DataAbstraction.CardTypeEnum;

namespace DataAbstraction
{
    public class CardEntity
    {
        public int Id { get; set; }
        public string HolderName { get; set; }
        public string Number { get; set; }
        public string CVVCode { get; set; }

        public CardType Type { get; set; }
        public CardSystem System { get; set; }

        public bool IsBlocked { get; set; } = false;

        public CardEntity()
        {

        }

        public CardEntity(int id, string holderName, string number, string cVVCode, CardType type, CardSystem system, bool isBlocked)
        {
            Id = id;
            HolderName = holderName;
            Number = number;
            CVVCode = cVVCode;
            Type = type;
            System = system;
            IsBlocked = isBlocked;
        }
        public CardEntity(string holderName, string number, string cVVCode, CardType type, CardSystem system, bool isBlocked)
        {
            HolderName = holderName;
            Number = number;
            CVVCode = cVVCode;
            Type = type;
            System = system;
            IsBlocked = isBlocked;
        }
        public CardEntity(string holderName, CardType type, CardSystem system)
        {
            HolderName = holderName;
            Number = GetCardNumber();
            CVVCode = GenerateCVV();
            Type = type;
            System = system;
        }


        Random random = new Random();
        private string GetCardNumber()
        {
            string result = "";

            for (int i = 0; i < 4; i++)
            {
                int code = random.Next(0, 9999);

                result = result + String.Format("{0:0000}", code);

                if (i < 3)
                {
                    result = result + " ";
                }
            }

            return result;
        }

        private string GenerateCVV()
        {
            int cvv = random.Next(0, 999);

            return String.Format("{0:000}", cvv);
        }
    }
}