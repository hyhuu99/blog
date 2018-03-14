using Autofac;
using Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.UnitOfWork;

namespace Database
{
    public class DatabaseModule:Module
    {
        private string connStr;
        public DatabaseModule(string connString)
        {
            this.connStr = connString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MiniBlog(connStr)).InstancePerRequest();
            builder
               .RegisterGeneric(typeof(GenericRepository<>))
               .As(typeof(IGenericRepository<>))
               .InstancePerDependency();
            builder
               .RegisterType(typeof(UnitOfWork.UnitOfWork))
               .As(typeof(IUnitOfWork))
               .InstancePerDependency();

            base.Load(builder);
        }
    }
}
