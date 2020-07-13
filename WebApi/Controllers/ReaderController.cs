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
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly IReaderService _readerService;

        public ReaderController(IReaderService service)
        {
            _readerService = service;
        }

        // GET: api/Reader
        [HttpGet]
        public ActionResult<IEnumerable<ReaderModel>> Get()
        {
            var readers = _readerService.GetAll();
            return Ok(readers);
        }

        //GET: api/Reader/DontReturnBooks
        [HttpGet("/DontReturnBooks", Name = "GetReadersThatDontReturnBooks")]
        public ActionResult<IEnumerable<ReaderModel>> GetReadersThatDontReturnBooks()
        {
            var readers = _readerService.GetReadersThatDontReturnBooks();
            return Ok(readers);
        }

        // GET: api/Reader/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<ReaderModel>> Get(int id)
        {
            var reader = await _readerService.GetByIdAsync(id);
            return Ok(reader);
        }

        // POST: api/Reader
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ReaderModel value)
        {
            await _readerService.AddAsync(value);
            return Ok();
        }

        // PUT: api/Reader/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] ReaderModel value)
        {
            await _readerService.UpdateAsync(value);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _readerService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}
