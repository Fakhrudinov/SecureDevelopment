using DataAbstraction;
using DataAbstraction.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataBaseRepositoryEF
{
    public class DataBaseRepositoryEF : IDataBaseRepositoryEF
    {
        private BankCardContext _context;

        public DataBaseRepositoryEF(BankCardContext context)
        {
            _context = context;
        }

        public Task<bool> CheckCardIdExist(int id)
        {
            var aaa = _context.CardEntities.Any(e => e.Id == id);
            if (aaa)
            {
                return Task.FromResult(true);
            }
            
            return Task.FromResult(false);
        }

        public async Task<IEnumerable<CardEntity>> GetAllCards()
        {
            return await _context.CardEntities.ToListAsync();
        }

        public async Task<CardEntity> GetCardById(int id)
        {
            return await _context.CardEntities.FindAsync(id);
        }

        public async Task<CardEntity> GetCardByNumber(string number)
        {
            return await _context.CardEntities
                .FirstOrDefaultAsync(card => card.Number.Equals(number));
        }

        public async Task EditCardEntity(CardEntity cardEntity)
        {
            CardEntity cardToEdit = await _context.CardEntities.FindAsync(cardEntity.Id);

            if (cardToEdit != null)
            {
                cardToEdit.HolderName = cardEntity.HolderName;
                cardToEdit.Number = cardEntity.Number;
                cardToEdit.CVVCode = cardEntity.CVVCode;
                cardToEdit.Type = cardEntity.Type;
                cardToEdit.System = cardEntity.System;
                cardToEdit.IsBlocked = cardEntity.IsBlocked;


                _context.CardEntities.Update(cardToEdit);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<CardEntity> CreateNewCard(CardEntityToPost cardEntity)
        {
            CardEntity newCard = new CardEntity(cardEntity.HolderName, cardEntity.Number, cardEntity.CVVCode, cardEntity.Type, cardEntity.System, cardEntity.IsBlocked);

            _context.CardEntities.Add(newCard);
            await _context.SaveChangesAsync();

            return newCard;
        }

        public async Task<CardEntity> CreateNewCardAutoField(CardEntityToPostAutoField cardEntity)
        {
            CardEntity newCard = new CardEntity(cardEntity.HolderName, cardEntity.Type, cardEntity.System);

            _context.CardEntities.Add(newCard);
            await _context.SaveChangesAsync();

            return newCard;
        }

        public async Task DeleteCardEntity(int id)
        {
            var cardEntity = await _context.CardEntities.FindAsync(id);
            if(cardEntity != null)
            {
                _context.CardEntities.Remove(cardEntity);

                await _context.SaveChangesAsync();
            }
        }
    }
}