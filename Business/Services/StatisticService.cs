using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public StatisticService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public IEnumerable<BookModel> GetMostPopularBooks(int bookCount)
        {
            var histories =_unit.HistoryRepository
                .GetAllWithDetails()
                .ToList();

            if (!histories.Any())
                return new List<BookModel>();

            return histories.GroupBy(x => x.BookId)
                .OrderByDescending(x => x.Count())
                .Take(bookCount)
                .Select(x => _mapper.Map<Book, BookModel>(x.FirstOrDefault().Book));
        }

        public IEnumerable<ReaderActivityModel> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate,
            DateTime lastDate)
        {
            var histories = _unit.HistoryRepository
                .GetAllWithDetails()
                .Where(x => x.TakeDate >= firstDate && x.ReturnDate <= lastDate)
                .ToList();

            if (!histories.Any())
                return new List<ReaderActivityModel>();

            return histories
                .GroupBy(x => x.Card.ReaderId)
                .OrderByDescending(x => x.Count())
                .Take(readersCount)
                .Select(x => new ReaderActivityModel
                    {BooksCount = x.Count(), ReaderId = x.Key, ReaderName = x.ElementAt(0).Card.Reader.Name});
        }
    }
}