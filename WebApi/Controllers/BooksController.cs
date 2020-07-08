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

        [HttpGet]
        public ActionResult<IEnumerable<BookModel>> GetBooks()
        {
            var books = _booksService.GetAll();

            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] BookModel bookModel)
        {
            await _booksService.AddAsync(bookModel);

            //return CreatedAtRoute("DefaultApi", new { id = bookModel.Id}, bookModel);
            return Ok();
        }
    }
}