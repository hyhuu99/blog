using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(int? id);
        void Insert(TEntity obj);
        void Delete(TEntity obj);
        void Update(TEntity obj);
        void EditsConcurrency(TEntity obj, byte[] RowVersion);
        IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> where);
    }
}
