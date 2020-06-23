using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IReaderService
    {
        IEnumerable<ReaderModel> GetAll();

        ReaderModel GetById(int id);

        void Add(ReaderModel reader);

        void Update(int readerId, ReaderModel reader);

        void Delete(int id);
    }
}