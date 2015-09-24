using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Data;
using Mob.Core.Domain;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Seo;
using Nop.Services.Seo;

namespace Mob.Core.Services
{
    public abstract class BaseEntityService<T> : IBaseEntityService<T> where T : BaseMobEntity
    {
        private readonly IMobRepository<T> _repository;
        private readonly IWorkContext _workContext;
        private readonly IUrlRecordService _urlRecordService;

        protected IMobRepository<T> Repository
        {
            get { return _repository; }
        }

        protected BaseEntityService(IMobRepository<T> repository)
        {
            _repository = repository;
        }

        protected BaseEntityService(IMobRepository<T> repository, IWorkContext workContext,
            IUrlRecordService urlRecordService) : this(repository)
        {
            _workContext = workContext;
            _urlRecordService = urlRecordService;
        }

        public void Insert(T entity)
        {
            if(entity.Id == 0)
                _repository.Insert(entity);

            if (entity is ISlugSupported && entity is INameSupported)
                InsertUrlRecord(entity);
        }

        public void Delete(T entity)
        {
            if(entity != null)
                _repository.Delete(entity);

            if (entity is ISlugSupported && entity is INameSupported)
            {
                // TODO: Need Nop UrlRecordService.GetByEntityId(entityId, entityName) method
                var currentSlug = _urlRecordService.GetActiveSlug(entity.Id, typeof(T).Name, _workContext.WorkingLanguage.Id);
                if (!string.IsNullOrEmpty(currentSlug))
                {
                    var urlRecord = _urlRecordService.GetBySlug(currentSlug);
                    _urlRecordService.DeleteUrlRecord(urlRecord);
                }
            }

        }

        public void Update(T entity)
        {
            if(entity.Id != 0)
                _repository.Update(entity);


            if (entity is ISlugSupported && entity is INameSupported)
            {
                var currentSlug = _urlRecordService.GetActiveSlug(entity.Id, typeof(T).Name, _workContext.WorkingLanguage.Id);

                if (!string.IsNullOrEmpty(currentSlug))
                    UpdateUrlRecord(entity, currentSlug);
                else
                    InsertUrlRecord(entity);
            }
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

        #region Helper Methods

        private void InsertUrlRecord(T entity)
        {
            var namedEntity = (INameSupported)entity;
            var urlRecord = new UrlRecord() {
                EntityId = entity.Id,
                EntityName = typeof(T).Name,
                IsActive = true,
                LanguageId = _workContext.WorkingLanguage.Id,
                Slug = Nop.Services.Seo.SeoExtensions.GetSeName(namedEntity.Name, true, false)
            };

            _urlRecordService.InsertUrlRecord(urlRecord);
        }

        private void UpdateUrlRecord(T entity, string currentSlug)
        {
            var urlRecord = _urlRecordService.GetBySlug(currentSlug);
            var namedEntity = (INameSupported)entity;
            var newSlug = Nop.Services.Seo.SeoExtensions.GetSeName(namedEntity.Name, true, false);

            urlRecord.EntityName = typeof(T).Name;
            urlRecord.LanguageId = _workContext.WorkingLanguage.Id;
            urlRecord.Slug = newSlug;

            _urlRecordService.UpdateUrlRecord(urlRecord);
        }
        #endregion
        
    }
}
