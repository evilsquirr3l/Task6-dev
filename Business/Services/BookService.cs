using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        
        public IEnumerable<BookModel> GetAll()
        {
            var books = _unit.BookRepository.FindAllWithDetails();

            return _mapper.Map<IEnumerable<BookModel>>(books);
        }

        public async Task AddAsync(BookModel model)
        {
            BookValidation.CheckBook(model);
            
            var book = _mapper.Map<Book>(model);
            
            await _unit.BookRepository.AddAsync(book);
            await _unit.SaveAsync();

            model.Id = book.Id;
        }

        public async Task<BookModel> GetByIdAsync(int id)
        {
            var book = await _unit.BookRepository.GetByIdWithDetailsAsync(id);

            return _mapper.Map<BookModel>(book);
        }

        public async Task UpdateAsync(BookModel model)
        {
            BookValidation.CheckBook(model);
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
            var books = _unit.BookRepository.FindAllWithDetails();

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