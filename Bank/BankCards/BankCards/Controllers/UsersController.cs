using AuthService;
using DataAbstraction;
using DataAbstraction.AuthModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataValidationService;
using System.Text;
using System.Security.Cryptography;
using BankCards.WorkWithForeignAssembly;
using System.Reflection;
using System.Threading;

namespace PersonsAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        private const string _userDelimiter = "kasjh12dfr4bj";

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(NewUser user)
        {
            var response = new ValidationResponseModel();

            // простые проверки
            UserEntityValidationService validator = new UserEntityValidationService();
            var validationResult = validator.Validate(user);

            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            int loginExist;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                loginExist = await _userService.GetUserByLoginAsync(user.Login, cts);
            }
            catch (OperationCanceledException)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"T_101.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }


            //check existance
            if (loginExist != 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"U_106.1 '{user.Login}' Такой пользователь уже есть");

                return UnprocessableEntity(response);
            }
            else
            {
                string hashed = HashThisUser(user.Login + _userDelimiter + user.Password);
                
                cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                try
                {
                    await _userService.CreateNewUserAsync(user.Login, hashed, cts);
                }
                catch (OperationCanceledException)
                {
                    response.IsValid = false;
                    response.ValidationMessages.Add($"T_101.2 TimeOut Error. Contact admin to investigate problem");
                    return UnprocessableEntity(response);
                }
                finally
                {
                    cts.Dispose();
                }

                return Ok();
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(NewUser user)
        {
            var response = new ValidationResponseModel();

            // простые проверки
            UserEntityValidationService validator = new UserEntityValidationService();
            var validationResult = validator.Validate(user);

            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            string hashed = HashThisUser(user.Login + _userDelimiter + user.Password);

            int userExist;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                userExist = await _userService.GetUserByLogonAsync(user.Login, hashed, cts);
            }
            catch (OperationCanceledException)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"T_102.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }


            //check existance
            if (userExist == 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"U_106.2 '{user.Login}' пользователь или пароль не корректен");

                return UnprocessableEntity(response);
            }
            else
            {
                cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                try
                {
                    var token = await _userService.Authentificate(user.Login, hashed, cts);

                    if (token is null)
                    {
                        response.IsValid = false;
                        response.ValidationMessages.Add($"U_107.1 для пользователя '{user.Login}' аутентификация не удалась");

                        return BadRequest(response);
                    }
                    SetTokenCookie(token.RefreshToken);
                    return Ok(token);
                }
                catch (OperationCanceledException)
                {
                    response.IsValid = false;
                    response.ValidationMessages.Add($"T_102.2 TimeOut Error. Contact admin to investigate problem");
                    return UnprocessableEntity(response);
                }
                finally
                {
                    cts.Dispose();
                }
            }
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh()
        {
            string oldRefreshToken = Request.Cookies["refreshToken"];

            string newRefreshToken;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                newRefreshToken = await _userService.RefreshToken(oldRefreshToken, cts);
            }
            catch (OperationCanceledException)
            {
                var response = new ValidationResponseModel();
                response.IsValid = false;
                response.ValidationMessages.Add($"T_103.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }


            if (string.IsNullOrWhiteSpace(newRefreshToken))
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            SetTokenCookie(newRefreshToken);

            return Ok(newRefreshToken);
        }



        [HttpGet("new/user")]
        [AllowAnonymous]
        public async Task<ActionResult<NewUser>> GetNewUser()
        {
            NewUser result = GetNewUserFromForeignLib();
            var response = new ValidationResponseModel();

            bool loginNotCreated = true;
            while (loginNotCreated)
            {
                int loginExist;
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                try
                {
                    loginExist = await _userService.GetUserByLoginAsync(result.Login, cts);
                }
                catch (OperationCanceledException)
                {
                    response.IsValid = false;
                    response.ValidationMessages.Add($"T_104.1 TimeOut Error. Contact admin to investigate problem");
                    return UnprocessableEntity(response);
                }
                finally
                {
                    cts.Dispose();
                }

                //check existance
                if (loginExist != 0) // exist, generate user again
                {
                    result = GetNewUserFromForeignLib();
                }
                else // ok, write new user to dataBase
                {
                    string hashed = HashThisUser(result.Login + _userDelimiter + result.Password);

                    cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromSeconds(5));
                    try
                    {
                        await _userService.CreateNewUserAsync(result.Login, hashed, cts);
                    }
                    catch (OperationCanceledException)
                    {
                        response.IsValid = false;
                        response.ValidationMessages.Add($"T_104.2 TimeOut Error. Contact admin to investigate problem");
                        return UnprocessableEntity(response);
                    }
                    finally
                    {
                        cts.Dispose();
                    }

                    loginNotCreated = false;
                }
            }
            return Ok(result);
        }

        private NewUser GetNewUserFromForeignLib()
        {
            NewUser result = AssemblyHandler.LoadAssembly();
            // очистка
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return result;
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private ValidationResponseModel SetResponseFromValidationResult(ValidationResult validationResultAsync, ValidationResponseModel response)
        {
            List<string> ValidationMessages = new List<string>();

            response.IsValid = false;
            foreach (ValidationFailure failure in validationResultAsync.Errors)
            {
                ValidationMessages.Add(failure.ErrorCode + " " + failure.ErrorMessage);
            }
            response.ValidationMessages = ValidationMessages;

            return response;
        }
        private string HashThisUser(string userData)
        {
            //Строка преобразуется в массив байтов.
            UTF8Encoding textConverter = new UTF8Encoding();
            byte[] passBytes = textConverter.GetBytes(userData);

            SHA384 shaM = SHA384.Create();

            shaM.ComputeHash(passBytes);
            string result = Convert.ToBase64String(shaM.Hash);
            return result;
        }
    }
}