using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        // [HttpGet]
        // public ActionResult<IEnumerable<BookModel>> GetBooks()
        // {
        //     var books = _booksService.GetAll();
        //
        //     return Ok(books);
        // }
        
        [HttpGet]
        public ActionResult<IEnumerable<BookModel>> GetByFilter([FromQuery] FilterSearchModel model)
        {
            var books = _booksService.GetByFilter(model);

            return Ok(books);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<BookModel>>> GetById(int id)
        {
            var book = await _booksService.GetByIdAsync(id);

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BookModel bookModel)
        {
            await _booksService.AddAsync(bookModel);

            //return CreatedAtRoute("DefaultApi", new { id = bookModel.Id}, bookModel);
            return Ok(bookModel);
        }

        [HttpPut]
        public async Task<ActionResult> Update(BookModel bookModel)
        {
            await _booksService.UpdateAsync(bookModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _booksService.DeleteByIdAsync(id);

            return Ok();
        }
    }
}