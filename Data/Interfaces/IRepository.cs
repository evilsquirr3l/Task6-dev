using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> FindAll();

        Task<TEntity> GetById(int id);
        
        Task AddAsync(TEntity entity);
        
        void Update(TEntity entity);
        
        void Delete(TEntity entity);

        Task DeleteById(int id);
    }
}