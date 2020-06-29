using System;
using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IBooksService : ICrudInterface<BookModel>
    {
        IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch);
        
        DateTime GetBookReturningDate(int bookId);

        bool IsBookReturned(int bookId);
    }
}