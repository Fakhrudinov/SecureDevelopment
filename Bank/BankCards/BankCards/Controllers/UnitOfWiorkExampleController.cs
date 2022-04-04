using DataAbstraction;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnitOfWorkRepoPattern;

namespace BankCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfWiorkExampleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWiorkExampleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Made a minimum implementation just to examine how its work
        /// </summary>
        /// <returns></returns>

        [HttpGet("BankCards/all")]
        public async Task<ActionResult<CardEntity>> GetAllCards()
        {
            var cards = await _unitOfWork.BankCards.GetAll();

            if (cards == null)
            {
                return UnprocessableEntity();
            }

            return Ok(cards);
        }

        [HttpGet("BankCard/byNumber/{number}")]
        public async Task<ActionResult<CardEntity>> GetCardEntityByNumber([FromRoute] string number)
        {
            var cardEntity = await _unitOfWork.BankCards.GetCardByNumber(number);

            //check existance
            if (cardEntity == null)
            {
                return UnprocessableEntity();
            }

            return cardEntity;
        }


        [HttpGet("Users/all")]
        public async Task<ActionResult<CardEntity>> GetAllUsers()
        {
            var users = await _unitOfWork.AuthUser.GetAll();

            if (users == null)
            {
                return UnprocessableEntity();
            }

            return Ok(users);
        }
    }
}
