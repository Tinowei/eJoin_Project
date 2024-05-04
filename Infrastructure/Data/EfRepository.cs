using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        //衍生類別可以使用，專用型Repository可以做交易
        protected EJoinContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        public EfRepository(EJoinContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            DbContext.SaveChanges();
            return entities;
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Any(expression);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.AnyAsync(expression);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.FirstOrDefault(expression);
        }
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.FirstOrDefaultAsync(expression);
        }

        public TEntity GetById<TId>(TId id)
        {
            return DbSet.Find(new object[] { id });
        }

        public async Task<TEntity> GetByIdAsync<TId>(TId id)
        {
            return await DbSet.FindAsync(new object[] { id });
        }

        public List<TEntity> List(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Where(expression).ToList();
        }

        public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Where(expression).ToListAsync();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.SingleOrDefault(expression);
        }
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.SingleOrDefaultAsync(expression);
        }
        public TEntity Update(TEntity entity)
        {
            DbSet.Update(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            DbContext.SaveChanges();
            return entities;
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            DbContext.SaveChanges();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
            DbContext.SaveChanges();
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();
            return entities;
        }
    }
}
