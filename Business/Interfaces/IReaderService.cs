using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IReaderService : ICrud<ReaderModel>
    {
        IEnumerable<ReaderModel> GetReadersThatDontReturnBooks();
    }
}