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
            var book = await _unit.BookRepository.GetByIdWithDetailsAsync(id);

            return _mapper.Map<BookModel>(book);
        }

        public async Task UpdateAsync(BookModel model)
        {
            var book = _mapper.Map<Book>(model);
            
            _unit.BookRepository.Update(book);
            await _unit.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unit.BookRepository.DeleteById(modelId);
            await _unit.SaveAsync();
        }

        public IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch)
        {
            var filteredBooks = _unit.BookRepository
                .GetAllWithDetails()
                .Where(b => b.Author == filterSearch.Author && b.Year == filterSearch.Year);

            return _mapper.Map<IEnumerable<BookModel>>(filteredBooks);
        }

        public DateTime GetBookReturningDate(int bookId)
        {
            var history = GetLastHistoryWhenBookWasTaken(bookId);

            return history.ReturnDate ?? history.TakeDate.AddMonths(1);
        }

        public bool IsBookReturned(int bookId)
        {
            var history = GetLastHistoryWhenBookWasTaken(bookId);

            if (history == null)
            {
                return false;
            }
            
            return history.ReturnDate < DateTime.Now;
        }

        private History GetLastHistoryWhenBookWasTaken(int bookId)
        {
            return _unit.HistoryRepository
                .FindAll()
                .OrderByDescending(h => h.Id)
                .FirstOrDefault(h => h.BookId == bookId);
        }
    }
}