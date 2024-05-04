using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 開始交易
        /// </summary>
        void Begin();

        /// <summary>
        /// 交易完成一定要呼叫才會存進去DB
        /// </summary>
        void Commit();
        /// <summary>
        /// 交易失敗流程要呼叫的
        /// </summary>
        void Rollback();

        /// <summary>
        /// 取得這次的unit of work 要用到的IRepository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> GetRepository<T>() where T : class;
    }
}
