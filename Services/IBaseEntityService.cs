using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Domain;

namespace Mob.Core.Services
{
    public interface IBaseEntityService<T> where T: BaseMobEntity
    {
        void Insert(T entity);

        void Delete(T entity);
        
        void Update(T entity);

        void Delete(Expression<Func<T, bool>> where);

        T GetById(int id);

        T GetBySeName(string SeName);

        List<T> GetAll();
        
        List<T> GetAll(string Term, int Count = 15, int Page = 1);

        T First(Expression<Func<T, bool>> where);

        T FirstOrDefault(Expression<Func<T, bool>> where);

        int Count(Expression<Func<T, bool>> where = null);

        IQueryable<T> Get(Expression<Func<T, bool>> where = null);
    }
}
