using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface ICardService
    {
        IEnumerable<CardModel> GetAll();

        CardModel GetById(int id);

        void Add(CardModel card);

        void Update(int cardId, CardModel card);

        void Delete(int id);

        IEnumerable<BookModel> GetBooksByCardId(int cardId);

        void TakeBook(int cartId, int bookId);

        void HandOverBook(int cartId, int bookId);
    }
}