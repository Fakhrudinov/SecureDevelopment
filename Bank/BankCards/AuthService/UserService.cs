using DataAbstraction.AuthModels;
using DataAbstraction.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService
{
    public sealed class UserService : IUserService
    {
        private IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public const string SecretCode = "secretsecretsecretsecretsecretsecret";

        public async Task<TokenResponse> Authentificate(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            TokenResponse tokenResponse = new TokenResponse();

            int userId = await _repository.GetUserByLogonAsync(login, password);

            if (userId > 0)
            {
                tokenResponse.Token = GenerateJwtToken(userId, 5);
                RefreshToken refreshToken = GenerateRefreshToken(userId);
                refreshToken.UserId = userId;

                //сохранить в БД RefreshToken новый refresh с UserId пользователя
                RefreshToken isExist = await _repository.GetRefreshTokenByUserIdAsync(refreshToken);
                if (isExist == null)
                {
                    await _repository.SetNewRefreshTokenAsync(refreshToken);
                }
                else
                {
                    await _repository.UpdateRefreshTokenAsync(refreshToken);
                }

                tokenResponse.RefreshToken = refreshToken.Token;
                return tokenResponse;
            }

            return null;
        }

        public async Task<string> RefreshToken(string token)
        {
            //из бд проверить наличие refresh по стринг token.
            //вернуть найденный токен
            RefreshToken isExist = await _repository.GetRefreshTokenByTokenIdAsync(token);

            if (string.CompareOrdinal(isExist.Token, token) == 0)
            {
                if (isExist.IsExpired is false)
                {
                    //обновить в БД RefreshToken 
                    RefreshToken refreshToken = GenerateRefreshToken(isExist.UserId);
                    refreshToken.UserId = isExist.UserId;

                    await _repository.UpdateRefreshTokenAsync(refreshToken);

                    //вернуть новый токен
                    return refreshToken.Token;
                }
            }

            return string.Empty;
        }

        private string GenerateJwtToken(int id, int minutes)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecretCode);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(int id)
        {
            RefreshToken refreshToken = new RefreshToken();
            refreshToken.Expires = DateTime.Now.AddMinutes(60);
            refreshToken.Token = GenerateJwtToken(id, 60);
            return refreshToken;
        }

        public async Task<int> GetUserByLogonAsync(string login, string password)
        {
            return await _repository.GetUserByLogonAsync(login, password);
        }

        public async Task CreateNewUserAsync(string login, string password)
        {
            await _repository.CreateNewUserAsync(login, password);
        }

        public async Task<int> GetUserByLoginAsync(string login)
        {
            return await _repository.GetUserByLoginAsync(login);
        }
    }
}