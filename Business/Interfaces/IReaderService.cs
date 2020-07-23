using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IReaderService : ICrud<ReaderModel>
    {
        IEnumerable<ReaderModel> GetReadersThatDontReturnBooks();
    }
}