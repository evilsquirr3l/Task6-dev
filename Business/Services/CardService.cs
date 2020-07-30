using System;
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
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public CardService(IUnitOfWork unit, IMapper mapper)
        {
            this._unit = unit;
            this._mapper = mapper;
        }


        public async Task AddAsync(CardModel model)
        {
            var card = _mapper.Map<Card>(model);

            await _unit.CardRepository.AddAsync(card);
            await _unit.SaveAsync();

            model.Id = card.Id;
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await _unit.CardRepository.DeleteByIdAsync(modelId);
            await _unit.SaveAsync();
        }

        public IEnumerable<CardModel> GetAll()
        {
            var cards = _unit.CardRepository.FindAllWithDetails().ToList();

            return _mapper.Map<IEnumerable<CardModel>>(cards);
        }

        public IEnumerable<BookModel> GetBooksByCardId(int cardId)
        {
            var books = _unit.BookRepository.FindAllWithDetails().Where(x => x.Id == cardId).ToList();

            return _mapper.Map<IEnumerable<BookModel>>(books);
        }

        public async Task<CardModel> GetByIdAsync(int id)
        {
            var card = await _unit.CardRepository.GetByIdWithDetailsAsync(id);

            return _mapper.Map<CardModel>(card);
        }

        public async Task HandOverBookAsync(int cardId, int bookId)
        {
            var history = _unit.HistoryRepository.FindAll()
                .Where(x => x.BookId == bookId && x.CardId == cardId)
                .OrderByDescending(x => x.ReturnDate)
                .FirstOrDefault();

            if (history == null)
                throw new LibraryException($"Book with id '{bookId}' was never taken to card with id '{cardId}'");

            if (history.ReturnDate != null || history.ReturnDate != default)
                throw new LibraryException($"Book with id '{bookId}' is already returned");

            history.ReturnDate = DateTime.Now;

            _unit.HistoryRepository.Update(history);
            await _unit.SaveAsync();
        }

        public async Task TakeBookAsync(int cardId, int bookId)
        {
            var book = await _unit.BookRepository.GetByIdAsync(bookId);
            var card = await _unit.CardRepository.GetByIdAsync(cardId);

            if (book == null)
                throw new LibraryException($"Book with id '{bookId}' was not found");

            if (card == null)
                throw new LibraryException($"Card with id '{cardId}' was not found");

            var history = GetLastHistoryWhenBookWasTaken(bookId);

            if (history != null && history.ReturnDate > DateTime.Now)
                throw new LibraryException($"Book with id '{bookId}' is already taken");

            history = new History { BookId = bookId, CardId = cardId, Book = book, Card = card, TakeDate = DateTime.Now };

            card.Books.Add(history);

            await _unit.SaveAsync();
        }

        private History GetLastHistoryWhenBookWasTaken(int bookId)
        {
            return _unit.HistoryRepository
                .FindAll()
                .OrderByDescending(h => h.Id)
                .FirstOrDefault(h => h.BookId == bookId);
        }

        public async Task UpdateAsync(CardModel model)
        {
            var card = _mapper.Map<Card>(model);

            _unit.CardRepository.Update(card);
            await _unit.SaveAsync();
        }
    }
}
