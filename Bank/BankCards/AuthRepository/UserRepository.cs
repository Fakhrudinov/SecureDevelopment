using DataAbstraction.AuthModels;
using DataAbstraction.Repository;
using DataBaseRepositoryEF;
using Microsoft.EntityFrameworkCore;

namespace AuthRepository
{
    public class UserRepository : IUserRepository
    {
        private BankCardContext _context;

        public UserRepository(BankCardContext context)
        {
            _context = context;
        }


        public async Task<int> GetUserByLogonAsync(string login, string password, CancellationTokenSource cts)
        {
            User findedUser = await _context.Users.Where(x => x.Login == login && x.Password == password).SingleOrDefaultAsync(cts.Token);

            if (findedUser == null)
                return 0;

            return findedUser.Id;
        }

        public async Task<int> GetUserByLoginAsync(string login, CancellationTokenSource cts)
        {
            User findedUser = (await _context.Users.Where(x => x.Login == login).FirstOrDefaultAsync(cts.Token));

            if (findedUser == null)
                return 0;

            return findedUser.Id;
        }

        public async Task<RefreshToken> GetRefreshTokenByUserIdAsync(RefreshToken refreshToken, CancellationTokenSource cts)
        {
            return await _context.RefreshTokens.Where(x => x.UserId == refreshToken.UserId).SingleOrDefaultAsync(cts.Token);
        }

        public async Task SetNewRefreshTokenAsync(RefreshToken refreshToken, CancellationTokenSource cts)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cts.Token);

            await _context.SaveChangesAsync(cts.Token);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationTokenSource cts)
        {
            RefreshToken tokenToEdit = await _context.RefreshTokens.Where(x => x.UserId == refreshToken.UserId).SingleOrDefaultAsync(cts.Token);

            if (tokenToEdit != null)
            {
                tokenToEdit.Token = refreshToken.Token;
                tokenToEdit.UserId = refreshToken.UserId;
                tokenToEdit.Expires = refreshToken.Expires;

                _context.RefreshTokens.Update(tokenToEdit);

                await _context.SaveChangesAsync(cts.Token);
            }
        }

        public async Task<RefreshToken> GetRefreshTokenByTokenIdAsync(string token, CancellationTokenSource cts)
        {
            return await _context.RefreshTokens.Where(x => x.Token == token).SingleOrDefaultAsync(cts.Token);
        }

        public async Task CreateNewUserAsync(string login, string password, CancellationTokenSource cts)
        {
            User newUser = new User();
            newUser.Login = login;
            newUser.Password = password;

            await _context.Users.AddAsync(newUser, cts.Token);
            await _context.SaveChangesAsync(cts.Token);
        }
    }
}