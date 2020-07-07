using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class BooksService : IBooksService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public BooksService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public IEnumerable<BookModel> GetAll()
        {
            var books = _unit.BookRepository.FindAll().ToList();

            return _mapper.Map<IEnumerable<BookModel>>(books);
        }

        public async Task AddAsync(BookModel model)
        {
            var book = _mapper.Map<Book>(model);

            await _unit.BookRepository.AddAsync(book);
            await _unit.SaveAsync();
        }

        public Task<BookModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int modelId, BookModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int modelId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch)
        {
            throw new NotImplementedException();
        }

        public DateTime GetBookReturningDate(int bookId)
        {
            throw new NotImplementedException();
        }

        public bool IsBookReturned(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}