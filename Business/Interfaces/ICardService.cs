using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface ICardService : ICrudInterface<CardModel>
    {
        IEnumerable<BookModel> GetBooksByCardId(int cardId);

        void TakeBook(int cartId, int bookId);

        void HandOverBook(int cartId, int bookId);
    }
}