using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        
        public Repository(LibraryDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }
 
        public IQueryable<TEntity> FindAll()
        {
            return _dbSet;
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }
 
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
 
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeleteById(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}