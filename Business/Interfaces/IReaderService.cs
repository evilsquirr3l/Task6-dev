using System.Collections.Generic;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IReaderService
    {
        IEnumerable<Reader> GetAll();

        Reader GetById(int id);

        void Add(Reader reader);

        void Update(int readerId, Reader reader);

        void Delete(int id);
    }
}