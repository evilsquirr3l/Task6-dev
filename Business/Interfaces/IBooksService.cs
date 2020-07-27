using System.Collections.Generic;
using Business.Models;

namespace Business.Interfaces
{
    public interface IBooksService : ICrud<BookModel>
    {
        IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch);
    }
}