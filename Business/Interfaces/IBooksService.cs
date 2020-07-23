using System;
using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IBooksService : ICrud<BookModel>
    {
        IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch);
    }
}