using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card> GetByIdWithBooksAsync(int id);
    }
}
