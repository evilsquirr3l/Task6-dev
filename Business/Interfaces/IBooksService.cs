using System.Collections.Generic;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IBooksService
    {
        IEnumerable<Book> GetAll();
        
        IEnumerable<Book> GetByFilter(FilterModel filter);
        
        Book GetById(int id);

        void Add(Book book);

        void Update(int bookId, Book book);

        void Delete(int id);
    }
}