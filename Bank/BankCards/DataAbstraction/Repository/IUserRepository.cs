using DataAbstraction.AuthModels;

namespace DataAbstraction.Repository
{
    public interface IUserRepository
    {
        Task<int> GetUserByLogonAsync(string login, string password, CancellationTokenSource cts);
        Task<int> GetUserByLoginAsync(string login, CancellationTokenSource cts);
        Task<RefreshToken> GetRefreshTokenByUserIdAsync(RefreshToken refreshToken, CancellationTokenSource cts);
        Task SetNewRefreshTokenAsync(RefreshToken refreshToken, CancellationTokenSource cts);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationTokenSource cts);
        Task<RefreshToken> GetRefreshTokenByTokenIdAsync(string token, CancellationTokenSource cts);
        Task CreateNewUserAsync(string login, string password, CancellationTokenSource cts);
    }
}
