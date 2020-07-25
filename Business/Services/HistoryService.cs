using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public HistoryService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public IEnumerable<BookModel> GetMostPopularBooks(int bookCount) =>
            _unit.HistoryRepository
                .GetAllWithDetails()
                .GroupBy(x => x.BookId)
                .OrderBy(x => x.Count())
                .Take(bookCount)
                .Select(x => _mapper.Map<Book, BookModel>(x.FirstOrDefault().Book));
           

        public IEnumerable<ReaderActivityModel> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate, DateTime lastDate) =>
            _unit.HistoryRepository
                .GetAllWithDetails()
                .Where(x => x.TakeDate >= firstDate && x.ReturnDate <= lastDate)
                .GroupBy(x => x.Card.Reader)
                .OrderBy(x => x.Count())
                .Take(readersCount)
                .Select(x => new ReaderActivityModel { BooksCount = x.Count(), ReaderId = x.Key.Id, ReaderName = x.Key.Name});
    }
}