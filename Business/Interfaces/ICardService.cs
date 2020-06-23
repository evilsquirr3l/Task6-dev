using System.Collections.Generic;
using Data.Entities;

namespace Business.Interfaces
{
    public interface ICardService
    {
        IEnumerable<Card> GetAll();

        Card GetById(int id);

        void Create(Card card);

        void Update(int cardId, Card card);

        void Delete(int id);

        IEnumerable<Book> GetBooksByCardId(int cardId);

        void AddBookToCard(int cartId, int bookId);

        void DeleteBookFromCard(int cartId, int bookId);
    }
}