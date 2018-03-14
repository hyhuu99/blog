using Database.Code;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Database.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        
        MiniBlog db;
        public UnitOfWork(MiniBlog _db)
        {
            db = _db;
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Commit()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public int SpecialCommit()
        {
            try
            {
                db.SaveChanges();
                return (int)StatusCode.SUCCESS;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Post)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    return (int)StatusCode.WASDELETED;
                }
                else
                {
                    var databaseValues = (Post)databaseEntry.ToObject();

                    if (databaseValues.Title != clientValues.Title)
                        return (int)StatusCode.TITLEWASCHANGED;
                    if (databaseValues.BlogContent != clientValues.BlogContent)
                        return (int)StatusCode.CONTENTWASCHANGED;
                    if (databaseValues.Tags != clientValues.Tags)
                        return (int)StatusCode.TAGWASCHANGED;
                }
            }
            return (int)StatusCode.FAIL;
        }
    }
}
