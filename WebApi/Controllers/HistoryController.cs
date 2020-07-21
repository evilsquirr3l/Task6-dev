using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/history")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet("popularBooks")]
        public ActionResult<IEnumerable<BookModel>> GetMostPopularBooks(int bookCount)
        {
            var books = _historyService.GetMostPopularBooks(bookCount);

            return Ok(books);
        }

        [HttpGet("biggestReaders")]
        public ActionResult<IEnumerable<ReaderActivityModel>> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate,
            DateTime lastDate)
        {
            var readersActivity = _historyService.GetReadersWhoTookTheMostBooks(readersCount, firstDate, lastDate);

            return Ok(readersActivity);
        }
    }
}