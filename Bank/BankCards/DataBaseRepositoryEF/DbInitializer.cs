using DataAbstraction;
using static DataAbstraction.CardTypeEnum;
using static DataAbstraction.CardSystemEnum;

namespace DataBaseRepositoryEF
{
    public class DbInitializer
    {
        public static void Initialize(BankCardContext context)
        {
            context.Database.EnsureCreated();

            if (context.CardEntities.Any())
            {
                return;
            }

            var cards = new CardEntity[]
            {
                new CardEntity("IVANOV I",  "0000 1111 2222 3234","132", CardType.Debet,    CardSystem.MasterCard,  false),
                new CardEntity("PETROV E",  "0000 1111 1324 1234","234", CardType.Credit,   CardSystem.Visa,        false),
                new CardEntity("SIDOROV S", "0000 1111 5673 6345","098", CardType.Debet,    CardSystem.МИР,         false),
                new CardEntity("SMIRNOV A", "0000 1111 2345 8746","342", CardType.Credit,   CardSystem.MasterCard,  true),
                new CardEntity("PUPKIN V",  "0000 1111 2389 0657","534", CardType.Debet,    CardSystem.Visa,        false)
            };
            foreach (CardEntity card in cards)
            {
                context.CardEntities.Add(card);
            }

            context.SaveChanges();
        }
    }
}
