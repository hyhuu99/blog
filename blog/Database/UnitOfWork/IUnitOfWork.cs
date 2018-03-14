using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        bool Commit();
        int SpecialCommit();
    }
}
