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

        public async Task<bool> CheckCardIdExist(int id, CancellationTokenSource cts)
        {
            var aaa = await _context.CardEntities
                .Where(e => e.Id == id)
                .AnyAsync(cts.Token);
            if (aaa)
            {
                return Task.FromResult(true).Result;
            }
            
            return Task.FromResult(false).Result;
        }

        public async Task<IEnumerable<CardEntity>> GetAllCards(CancellationTokenSource cts)
        {
            return await _context.CardEntities.ToListAsync(cts.Token);
        }

        public async Task<CardEntity> GetCardById(int id, CancellationTokenSource cts)
        {
            return await _context.CardEntities
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync(cts.Token);
        }

        public async Task<CardEntity> GetCardByNumber(string number, CancellationTokenSource cts)
        {
            return await _context.CardEntities
                .Where(card => card.Number.Equals(number))
                .FirstOrDefaultAsync(cts.Token);
        }

        public async Task EditCardEntity(CardEntity cardEntity, CancellationTokenSource cts)
        {
            CardEntity cardToEdit = await _context.CardEntities
                .Where(e => e.Id == cardEntity.Id)
                .FirstOrDefaultAsync(cts.Token);

            if (cardToEdit != null)
            {
                cardToEdit.HolderName = cardEntity.HolderName;
                cardToEdit.Number = cardEntity.Number;
                cardToEdit.CVVCode = cardEntity.CVVCode;
                cardToEdit.Type = cardEntity.Type;
                cardToEdit.System = cardEntity.System;
                cardToEdit.IsBlocked = cardEntity.IsBlocked;


                _context.CardEntities.Update(cardToEdit);

                await _context.SaveChangesAsync(cts.Token);
            }
        }

        public async Task<CardEntity> CreateNewCard(CardEntityToPost cardEntity, CancellationTokenSource cts)
        {
            CardEntity newCard = new CardEntity(cardEntity.HolderName, cardEntity.Number, cardEntity.CVVCode, cardEntity.Type, cardEntity.System, cardEntity.IsBlocked);

            await _context.CardEntities.AddAsync(newCard, cts.Token);
            await _context.SaveChangesAsync(cts.Token);

            return newCard;
        }

        public async Task<CardEntity> CreateNewCardAutoField(CardEntityToPostAutoField cardEntity, CancellationTokenSource cts)
        {
            CardEntity newCard = new CardEntity(cardEntity.HolderName, cardEntity.Type, cardEntity.System);

            await _context.CardEntities.AddAsync(newCard, cts.Token);
            await _context.SaveChangesAsync(cts.Token);

            return newCard;
        }

        public async Task DeleteCardEntity(int id, CancellationTokenSource cts)
        {
            var cardEntity = await _context.CardEntities
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync(cts.Token);

            if (cardEntity != null)
            {
                _context.CardEntities.Remove(cardEntity);

                await _context.SaveChangesAsync(cts.Token);
            }
        }
    }
}