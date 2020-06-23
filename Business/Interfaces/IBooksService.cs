using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IBooksService
    {
        IEnumerable<BookModel> GetAll();
        
        IEnumerable<BookModel> GetByFilter(FilterModel filter);
        
        Book GetById(int id);

        void Add(BookModel book);

        void Update(int bookId, BookModel book);

        void Delete(int id);
    }
}