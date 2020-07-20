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

        public async Task<BookModel> GetByIdAsync(int id)
        {
            var book = await _unit.BookRepository.GetByIdAsync(id);

            return _mapper.Map<BookModel>(book);
        }

        public async Task UpdateAsync(BookModel model)
        {
            var book = _mapper.Map<Book>(model);
            
            _unit.BookRepository.Update(book);
            await _unit.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await _unit.BookRepository.DeleteByIdAsync(modelId);
            await _unit.SaveAsync();
        }

        public IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch)
        {
            var books = _unit.BookRepository.FindAll();

            if (!string.IsNullOrEmpty(filterSearch.Author))
            {
                books = books.Where(b => b.Author == filterSearch.Author);
            }

            if (filterSearch.Year != null)
            {
                books = books.Where(b => b.Year == filterSearch.Year);
            }

            return _mapper.Map<IEnumerable<BookModel>>(books);
        }
    }
}