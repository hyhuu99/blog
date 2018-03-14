using Database.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private MiniBlog db;
        public GenericRepository(MiniBlog _db)
        {
            db = _db;
        }
        public IQueryable<TEntity> GetAll()
        {          
                return db.Set<TEntity>();     
        }
        public TEntity GetById(int? id)
        {
            return db.Set<TEntity>().Find(id);
        }
        public void Insert(TEntity obj)
        {
                db.Entry(obj).State = EntityState.Added;
        }
        public void Delete(TEntity obj)
        {
                db.Entry(obj).State = EntityState.Deleted;
        }

        public void Update(TEntity obj)
        {
                db.Entry(obj).State = EntityState.Modified;
        }
        public void EditsConcurrency(TEntity obj,byte[] RowVersion)
        {               
                db.Entry(obj).OriginalValues["RowVersion"] = RowVersion;
            //obj.GetType().GetProperty("RowVersion").GetValue(obj, null)

        }
        public IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> where)
        {
                return db.Set<TEntity>().Where(where);          
        }

    }
}
