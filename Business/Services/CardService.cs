using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public async Task<int> AddAsync(CardModel model)
        {
            var card = mapper.Map<Card>(model);

            await unit.CardRepository.AddAsync(card);
            await unit.SaveAsync();

            return card.Id;
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
            var card = await unit.CardRepository.GetByIdWithBooksAsync(cardId);

            return mapper.Map<IEnumerable<BookModel>>(card.Books.Select(x => x.Book));
        }

        public async Task<CardModel> GetByIdAsync(int id)
        {
            var card = await unit.CardRepository.GetByIdWithBooksAsync(id);

            return mapper.Map<CardModel>(card);
        }

        public async Task HandOverBookAsync(int cardId, int bookId)
        {
            var history = unit.HistoryRepository.FindAll().Where(x => x.BookId == bookId && x.CardId == cardId).FirstOrDefault();

            //TODO: discuss exceptions with team
            if (history == null)
                throw new Exception($"History with book id '{bookId}' and card id '{cardId}' was not found");

            history.ReturnDate = DateTime.Now;

            unit.HistoryRepository.Update(history);
            await unit.SaveAsync();
        }

        public async Task TakeBookAsync(int cardId, int bookId)
        {
            var book = await unit.BookRepository.GetByIdAsync(bookId);
            var card = await unit.CardRepository.GetByIdAsync(cardId);

            //TODO: discuss exceptions with team
            if (book == null)
                throw new Exception($"Book with id '{bookId}' was not found");

            if (card == null)
                throw new Exception($"Card with id '{cardId}' was not found");

            var history = GetLastHistoryWhenBookWasTaken(bookId);

            if (history != null && history.ReturnDate > DateTime.Now)
                throw new Exception($"Book with id '{bookId}' is already taken");

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
