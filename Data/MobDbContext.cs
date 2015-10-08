using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;

namespace Mob.Core.Data
{
    public class MobDbContext : DbContext, IDbContext
    {
        public MobDbContext()
        {
            
        }
        public MobDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }
       
        public string CreateDatabaseInstallationScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public virtual void Install()
        {
            string script = CreateDatabaseInstallationScript();
            var sqls = script.Split(';');
            foreach (var sql in sqls)
            {
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    Database.ExecuteSqlCommand(sql);
                }
            }
            SaveChanges();
        }

        public virtual void Uninstall()
        {

        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : Nop.Core.BaseEntity
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : Nop.Core.BaseEntity, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        public bool ProxyCreationEnabled
        {
            get { return this.Configuration.ProxyCreationEnabled; }
            set { this.Configuration.ProxyCreationEnabled = value; }
        }

        public bool AutoDetectChangesEnabled
        {
            get { return this.Configuration.AutoDetectChangesEnabled; }
            set { this.Configuration.AutoDetectChangesEnabled = value; }
        }
    }
}
