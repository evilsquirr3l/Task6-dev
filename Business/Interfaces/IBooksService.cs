using System;
using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IBooksService
    {
        IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch);
        
        DateTime GetBookReturningDate(int bookId);

        bool IsBookReturned(int bookId);
        
        IEnumerable<BookModel> GetAll();
        
        BookModel GetById(int id);

        void Add(BookModel book);

        void Update(int bookId, BookModel book);

        void Delete(int id);
    }
}