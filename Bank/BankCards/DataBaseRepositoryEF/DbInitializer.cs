using DataAbstraction;
using static DataAbstraction.CardTypeEnum;
using static DataAbstraction.CardSystemEnum;
using DataAbstraction.AuthModels;

namespace DataBaseRepositoryEF
{
    public class DbInitializer
    {
        public static void Initialize(BankCardContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User { Login = "user1", Password = "bLvfochpY5GV1k1k1PYMQ0cjKr8x+uWfzqI8+q0cUdOqV1AV4p9S6FgShRLZAQ7I" }, //1qazXSW@
                new User { Login = "user2", Password = "HL1eVoOpCjgrpfRcFkoKvJPusK/XdEE1HEdYs5uXRSN9RnZHJTq0IrBpPqirNiAh" }  //1qazXSW@
            };
            foreach (User user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();


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
