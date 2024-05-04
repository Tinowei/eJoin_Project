using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private bool disposedValue;
        private readonly EJoinContext _eJoinContext;
        private readonly Dictionary<Type, object> _repositories;
        public EfUnitOfWork(EJoinContext eJoinContext)
        {
            _repositories = new Dictionary<Type, object>();
            _eJoinContext = eJoinContext;
        }
        public IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfEntity = typeof(T);
            if (_repositories.TryGetValue(typeOfEntity, out var existedRepository))
            {
                return (IRepository<T>)existedRepository;
            }
            var repository = new EfRepository<T>(_eJoinContext);
            _repositories.Add(typeOfEntity, repository);
            return repository;
        }
        public void Commit()
        {
            _eJoinContext.Database.CommitTransaction();
        }
        public void Rollback()
        {
            _eJoinContext.Database.RollbackTransaction();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _eJoinContext.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        public void Begin()
        {
            _eJoinContext.Database.BeginTransaction();
        }
    }
}
