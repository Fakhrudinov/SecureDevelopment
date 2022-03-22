using DataAbstraction.AuthModels;

namespace AuthService
{
    public interface IUserService
    {
        Task<TokenResponse> Authentificate(string user, string password, CancellationTokenSource cts);
        Task<string> RefreshToken(string token, CancellationTokenSource cts);
        Task<int> GetUserByLogonAsync(string login, string password, CancellationTokenSource cts);
        Task<int> GetUserByLoginAsync(string login, CancellationTokenSource cts);
        Task CreateNewUserAsync(string login, string password, CancellationTokenSource cts);
    }
}
