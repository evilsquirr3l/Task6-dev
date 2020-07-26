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
        private readonly IUnitOfWork unit;
        private readonly IMapper mapper;

        public CardService(IUnitOfWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }


        public async Task AddAsync(CardModel model)
        {
            var card = mapper.Map<Card>(model);

            await unit.CardRepository.AddAsync(card);
            await unit.SaveAsync();

            model.Id = card.Id;
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await unit.CardRepository.DeleteByIdAsync(modelId);
            await unit.SaveAsync();
        }

        public IEnumerable<CardModel> GetAll()
        {
            var cards = unit.CardRepository.FindAll().ToList();

            return mapper.Map<IEnumerable<CardModel>>(cards);
        }

        public async Task<IEnumerable<BookModel>> GetBooksByCardIdAsync(int cardId)
        {
            var books = unit.BookRepository.FindAll().Where(x => x.Id == cardId).ToList();

            return mapper.Map<IEnumerable<BookModel>>(books);
        }

        public async Task<CardModel> GetByIdAsync(int id)
        {
            var card = await unit.CardRepository.GetByIdWithBooksAsync(id);

            return mapper.Map<CardModel>(card);
        }

        public async Task HandOverBookAsync(int cardId, int bookId)
        {
            var history = unit.HistoryRepository.FindAll()
                .Where(x => x.BookId == bookId && x.CardId == cardId)
                .OrderByDescending(x => x.ReturnDate)
                .FirstOrDefault();

            if (history == null)
                throw new LibraryException($"Book with id '{bookId}' was never taken to card with id '{cardId}'");

            if (history.ReturnDate != null || history.ReturnDate != default)
                throw new LibraryException($"Book with id '{bookId}' is already returned");

            history.ReturnDate = DateTime.Now;

            unit.HistoryRepository.Update(history);
            await unit.SaveAsync();
        }

        public async Task TakeBookAsync(int cardId, int bookId)
        {
            var book = await unit.BookRepository.GetByIdAsync(bookId);
            var card = await unit.CardRepository.GetByIdAsync(cardId);

            if (book == null)
                throw new LibraryException($"Book with id '{bookId}' was not found");

            if (card == null)
                throw new LibraryException($"Card with id '{cardId}' was not found");

            var history = GetLastHistoryWhenBookWasTaken(bookId);

            if (history != null && history.ReturnDate > DateTime.Now)
                throw new LibraryException($"Book with id '{bookId}' is already taken");

            history = new History { BookId = bookId, CardId = cardId, Book = book, Card = card, TakeDate = DateTime.Now };

            card.Books.Add(history);

            await unit.SaveAsync();
        }

        private History GetLastHistoryWhenBookWasTaken(int bookId)
        {
            return unit.HistoryRepository
                .FindAll()
                .OrderByDescending(h => h.Id)
                .FirstOrDefault(h => h.BookId == bookId);
        }

        public async Task UpdateAsync(CardModel model)
        {
            var card = mapper.Map<Card>(model);

            unit.CardRepository.Update(card);
            await unit.SaveAsync();
        }
    }
}
