using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService cardsService;

        public CardsController(ICardService cardsService)
        {
            this.cardsService = cardsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookModel>> GetAll()
        {
            var cards = cardsService.GetAll();

            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CardModel>>> GetById(int id)
        {
            var card = await cardsService.GetByIdAsync(id);

            if (card == null)
            {
                return NotFound();
            }

            return Ok(card);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CardModel cardModel)
        {
            try
            {
                await cardsService.AddAsync(cardModel);
            }
            catch (LibraryException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(Add), cardModel);
        }

        [HttpPut]
        public async Task<ActionResult> Update(CardModel cardModel)
        {
            await cardsService.UpdateAsync(cardModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await cardsService.DeleteByIdAsync(id);

            return Ok();
        }

        [HttpGet("{id}/books")]
        public async Task<ActionResult> GetBooksByCardId(int id)
        {
            var books = await cardsService.GetBooksByCardIdAsync(id);

            return Ok(books);
        }

        [HttpPost("{cardId}/books/{bookId}")]
        public async Task<ActionResult> TakeBook(int cardId, int bookId)
        {
            try
            {
                await cardsService.TakeBookAsync(cardId, bookId);
            }
            catch (LibraryException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete("{cardId}/books/{bookId}")]
        public async Task<ActionResult> HandOverBook(int cardId, int bookId)
        {
            try
            {
                await cardsService.HandOverBookAsync(cardId, bookId);
            }
            catch (LibraryException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
