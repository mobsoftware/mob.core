using System;
using System.Collections.Generic;
using System.Linq;
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
        
        T GetById(int id);
        
        List<T> GetAll();
        
        List<T> GetAll(string Term, int Count = 15, int Page = 1);
    }
}
