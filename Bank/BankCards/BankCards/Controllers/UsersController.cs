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

namespace PersonsAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

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

            int loginExist = await _userService.GetUserByLoginAsync(user.Login);
            //check existance
            if (loginExist != 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"U_106.1 '{user.Login}' Такой пользователь уже есть");

                return UnprocessableEntity(response);
            }
            else
            {
                await _userService.CreateNewUserAsync(user.Login, user.Password);
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


            int userExist = await _userService.GetUserByLogonAsync(user.Login, user.Password);
            //check existance
            if (userExist == 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"U_106.1 '{user.Login}' пользователь или пароль не корректен");

                return UnprocessableEntity(response);
            }
            else
            {
                var token = await _userService.Authentificate(user.Login, user.Password);
                if (token is null)
                {
                    response.IsValid = false;
                    response.ValidationMessages.Add($"U_107.1 для пользователя '{user.Login}' аутентификация не удалась");

                    return BadRequest(response);
                }
                SetTokenCookie(token.RefreshToken);
                return Ok(token);
            }
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh()
        {
            string oldRefreshToken = Request.Cookies["refreshToken"];
            string newRefreshToken = await _userService.RefreshToken(oldRefreshToken);

            if (string.IsNullOrWhiteSpace(newRefreshToken))
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            SetTokenCookie(newRefreshToken);

            return Ok(newRefreshToken);
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
    }
}