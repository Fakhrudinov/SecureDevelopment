using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAbstraction;
using DataAbstraction.Repository;
using Microsoft.Extensions.Options;
using DataValidationService;
using FluentValidation.Results;

namespace BankCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        public SelectRepositorySettings _repoSettings { get; set; }
        private IRepository _repository;

        public CardsController(IDataBaseRepositoryEF repositoryEF, IDataBaseRepository repository, IOptions<SelectRepositorySettings> repoSettings)
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
        }

        // GET: api/Cards
        [HttpGet("AllCards")]
        public async Task<IEnumerable<CardEntity>> GetCardEntities()
        {
            var allCards = await _repository.GetAllCards();
            return allCards;
        }

        // GET: api/Cards/5
        [HttpGet("id/{id}")]
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


            CardEntity cardEntity = await _repository.GetCardById(id);

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


            CardEntity cardEntity = await _repository.GetCardByNumber(number);
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

            bool idExist = await _repository.CheckCardIdExist(cardEntity.Id);
            //check existance
            if (!idExist)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"C_102.1 '{cardEntity.Id}' Карты с таким Id не существует.");

                return UnprocessableEntity(response);
            }
            else
            {
                await _repository.EditCardEntity(cardEntity);

                return Ok(cardEntity);
            }
        }

        [HttpPost("CreateNew")]
        public async Task<ActionResult<CardEntity>> CreateCardEntity([FromBody] CardEntityToPost cardEntity)
        {
            var response = new ValidationResponseModel();
            CardEntityValidationService validator = new CardEntityValidationService();
            CardEntity cardCheck = new CardEntity();
            cardCheck.Id = 1;
            cardCheck.HolderName = cardEntity.HolderName;
            cardCheck.Number = cardEntity.Number;
            cardCheck.CVVCode = cardEntity.CVVCode;
            cardCheck.Type = cardEntity.Type;
            cardCheck.System = cardEntity.System;
            cardCheck.IsBlocked = cardEntity.IsBlocked;

            // check format
            var validationResult = validator.Validate(cardCheck);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            CardEntity newCard = await _repository.CreateNewCard(cardEntity);

            return Ok(newCard);
        }

        [HttpPost("CreateNewAutoField")]
        public async Task<ActionResult<CardEntity>> CreateCardEntityAutoField([FromBody] CardEntityToPostAutoField cardEntity)
        {
            var response = new ValidationResponseModel();
            CardEntityValidationService validator = new CardEntityValidationService();
            CardEntity cardCheck = new CardEntity();
            cardCheck.Id = 1;
            cardCheck.HolderName = cardEntity.HolderName;
            cardCheck.Type = cardEntity.Type;
            cardCheck.System = cardEntity.System;

            // check format
            var validationResult = validator.Validate(cardCheck);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }


            CardEntity newCard = await _repository.CreateNewCardAutoField(cardEntity);
            return Ok(newCard);
        }

        // DELETE: api/Cards/5
        [HttpDelete("delete/id/{id}")]
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


            bool idExist = await _repository.CheckCardIdExist(id);
            //check existance
            if (!idExist)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"C_103.1 '{id}' Карты с таким Id не существует.");

                return UnprocessableEntity(response);
            }
            else
            {
                await _repository.DeleteCardEntity(id);

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
