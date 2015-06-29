using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Domain;
using Nop.Core.Data;

namespace Mob.Core.Services
{
    public abstract class BaseEntityService<T> : IBaseEntityService<T> where T : BaseMobEntity
    {
        private readonly IRepository<T> _repository;

        public BaseEntityService(IRepository<T> repository)
        {
            _repository = repository;
        } 
        public void Insert(T entity)
        {
            if(entity.Id == 0)
                _repository.Insert(entity);
        }

        public void Delete(T entity)
        {
            if(entity != null)
                _repository.Delete(entity);
        }

        public void Update(T entity)
        {
            if(entity.Id == 0)
                _repository.Update(entity);
        }

        public T GetById(int id)
        {
            return _repository.Table.FirstOrDefault(m => m.Id == id);
        }

        public List<T> GetAll()
        {
            return _repository.Table.ToList();
        }

        public abstract List<T> GetAll(string Term, int Count = 15, int Page = 1);
        
    }
}
