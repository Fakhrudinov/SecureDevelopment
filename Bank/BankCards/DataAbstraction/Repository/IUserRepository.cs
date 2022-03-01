using DataAbstraction.AuthModels;

namespace DataAbstraction.Repository
{
    public interface IUserRepository
    {
        Task<int> GetUserByLogonAsync(string login, string password);
        Task<int> GetUserByLoginAsync(string login);
        Task<RefreshToken> GetRefreshTokenByUserIdAsync(RefreshToken refreshToken);
        Task SetNewRefreshTokenAsync(RefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByTokenIdAsync(string token);
        Task CreateNewUserAsync(string login, string password);
    }
}
