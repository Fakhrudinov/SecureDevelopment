using DataAbstraction.AuthModels;

namespace AuthService
{
    public interface IUserService
    {
        Task<TokenResponse> Authentificate(string user, string password);
        Task<string> RefreshToken(string token);
        Task<int> GetUserByLogonAsync(string login, string password);
        Task<int> GetUserByLoginAsync(string login);
        Task CreateNewUserAsync(string login, string password);
    }
}
