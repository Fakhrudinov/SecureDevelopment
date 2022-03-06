using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAbstraction;
using DataAbstraction.Repository;
using Microsoft.Extensions.Options;
using DataValidationService;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using System;
using AutoMapper;

namespace BankCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        public SelectRepositorySettings _repoSettings { get; set; }
        private IRepository _repository;
        private readonly IMapper _autoMapper; 

        public CardsController(
            IDataBaseRepositoryEF repositoryEF, 
            IDataBaseRepository repository, 
            IOptions<SelectRepositorySettings> repoSettings,
            IMapper autoMapper)
        {
            _repoSettings = repoSettings.Value;

            if (_repoSettings.UseEntityFramework)
            {
                _repository = repositoryEF;                
            }
            else
            {
                _repository = repository;
            }
            
            _autoMapper = autoMapper;
        }

        // GET: api/Cards
        [HttpGet("AllCards")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CardEntity>>> GetCardEntities()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                var allCards = await _repository.GetAllCards(cts);
                return Ok(allCards);
            }
            catch (OperationCanceledException)
            {
                var response = new ValidationResponseModel();
                response.IsValid = false;
                response.ValidationMessages.Add($"T_201.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }
        }

        // GET: api/Cards/5
        [HttpGet("id/{id}")]
        [Authorize]
        public async Task<ActionResult<CardEntity>> GetCardEntityById([FromRoute] int id)
        {
            var response = new ValidationResponseModel();
            CardEntityValidationService validator = new CardEntityValidationService();
            CardEntity cardCheck = new CardEntity();
            cardCheck.Id = id;

            // check format
            var validationResult = validator.Validate(cardCheck);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            CardEntity cardEntity = null;

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                cardEntity = await _repository.GetCardById(id, cts);
            }
            catch (OperationCanceledException)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"T_202.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }

            //check existance
            if (cardEntity == null)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"C_100.1 '{id}' Карты с таким Id не существует.");

                return UnprocessableEntity(response);
            }

            return cardEntity;
        }

        // GET: api/Cards/number/0000 1111 5673 6345
        [HttpGet("number/{number}")]
        [Authorize]
        public async Task<ActionResult<CardEntity>> GetCardEntityByNumber([FromRoute] string number)
        {
            var response = new ValidationResponseModel();
            CardEntityValidationService validator = new CardEntityValidationService();
            CardEntity cardCheck = new CardEntity();
            cardCheck.Id = 1;
            cardCheck.Number = number;

            // check format
            var validationResult = validator.Validate(cardCheck);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            CardEntity cardEntity = null;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                cardEntity = await _repository.GetCardByNumber(number, cts);
            }
            catch (OperationCanceledException)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"T_203.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }

            //check existance
            if (cardEntity == null)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"C_101.1 '{number}' Карты с таким номером не существует.");

                return UnprocessableEntity(response);
            }

            return cardEntity;
        }

        [HttpPut("Edit")]
        [Authorize]
        public async Task<ActionResult<CardEntity>> EditCardEntity([FromBody] CardEntity cardEntity)
        {
            CardEntityValidationService validator = new CardEntityValidationService();
            var response = new ValidationResponseModel();

            ValidationResult validationResult = validator.Validate(cardEntity);

            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            bool idExist = false;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                idExist = await _repository.CheckCardIdExist(cardEntity.Id, cts);
            }
            catch (OperationCanceledException)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"T_204.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }

            //check existance
            if (!idExist)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"C_102.1 '{cardEntity.Id}' Карты с таким Id не существует.");

                return UnprocessableEntity(response);
            }
            else
            {
                cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                try
                {
                    await _repository.EditCardEntity(cardEntity, cts);
                }
                catch (OperationCanceledException)
                {
                    response.IsValid = false;
                    response.ValidationMessages.Add($"T_204.2 TimeOut Error. Contact admin to investigate problem");
                    return UnprocessableEntity(response);
                }
                finally
                {
                    cts.Dispose();
                }

                return Ok(cardEntity);
            }
        }

        [HttpPost("CreateNew")]
        [Authorize]
        public async Task<ActionResult<CardEntity>> CreateCardEntity([FromBody] CardEntityToPost cardEntity)
        {
            var response = new ValidationResponseModel();
            CardEntityValidationService validator = new CardEntityValidationService();
            CardEntity cardCheck = new CardEntity();
            // mapping manually:
            //cardCheck.HolderName = cardEntity.HolderName;
            //cardCheck.Number = cardEntity.Number;
            //cardCheck.CVVCode = cardEntity.CVVCode;
            //cardCheck.Type = cardEntity.Type;
            //cardCheck.System = cardEntity.System;
            //cardCheck.IsBlocked = cardEntity.IsBlocked;

            //mapping by AutoMap:
            cardCheck = _autoMapper.Map<CardEntity>(cardEntity);
            cardCheck.Id = 1;

            // check format
            var validationResult = validator.Validate(cardCheck);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            CardEntity newCard = null;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                newCard = await _repository.CreateNewCard(cardEntity, cts);
            }
            catch (OperationCanceledException)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"T_205.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }
            return Ok(newCard);
        }

        [HttpPost("CreateNewAutoField")]
        [Authorize]
        public async Task<ActionResult<CardEntity>> CreateCardEntityAutoField([FromBody] CardEntityToPostAutoField cardEntity)
        {
            var response = new ValidationResponseModel();
            CardEntityValidationService validator = new CardEntityValidationService();
            CardEntity cardCheck = new CardEntity();
            // mapping manually:
            //cardCheck.HolderName = cardEntity.HolderName;
            //cardCheck.Type = cardEntity.Type;
            //cardCheck.System = cardEntity.System;

            //mapping by AutoMap:
            cardCheck = _autoMapper.Map<CardEntity>(cardEntity);
            cardCheck.Id = 1;

            // check format
            var validationResult = validator.Validate(cardCheck);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            CardEntity newCard = null;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                newCard = await _repository.CreateNewCardAutoField(cardEntity, cts);
            }
            catch (OperationCanceledException)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"T_206.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }

            return Ok(newCard);
        }

        // DELETE: api/Cards/5
        [HttpDelete("delete/id/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCardEntity([FromRoute] int id)
        {
            var response = new ValidationResponseModel();
            CardEntityValidationService validator = new CardEntityValidationService();
            CardEntity cardCheckId = new CardEntity();
            cardCheckId.Id = id;

            // check format
            var validationResult = validator.Validate(cardCheckId);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            bool idExist = false;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                idExist = await _repository.CheckCardIdExist(id, cts);
            }
            catch (OperationCanceledException)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"T_207.1 TimeOut Error. Contact admin to investigate problem");
                return UnprocessableEntity(response);
            }
            finally
            {
                cts.Dispose();
            }

            //check existance
            if (!idExist)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"C_103.1 '{id}' Карты с таким Id не существует.");

                return UnprocessableEntity(response);
            }
            else
            {
                cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                try
                {
                    await _repository.DeleteCardEntity(id, cts);
                }
                catch (OperationCanceledException)
                {
                    response.IsValid = false;
                    response.ValidationMessages.Add($"T_207.2 TimeOut Error. Contact admin to investigate problem");
                    return UnprocessableEntity(response);
                }
                finally
                {
                    cts.Dispose();
                }

                return Ok();
            }
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
