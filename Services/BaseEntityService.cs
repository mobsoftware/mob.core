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
using Nop.Core.Infrastructure;
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

        public T GetBySeName(string SeName)
        {
            //TODO: There should be a method to retrieve url records with entity type and slug
            var urlRecords = _urlRecordService.GetAllUrlRecords(SeName);
            var entityUrlRecord = urlRecords.FirstOrDefault(x => x.EntityName == typeof(T).Name);

            if (entityUrlRecord == null)
                return null;

            //resolve the service according to the type of T
            var tEntityService = EngineContext.Current.Resolve<IBaseEntityService<T>>();
            return tEntityService.GetById(entityUrlRecord.EntityId);
        }

        public List<T> GetAll()
        {
            return _repository.Table.ToList();
        }

        public abstract List<T> GetAll(string Term, int Count = 15, int Page = 1);

        #region Helper Methods

        /// <summary>
        /// Local copy of Nop.Services.Seo.SeoExtensions.ValidateSeName. The existing method couldn't be used directly because it requires the entity to extend
        /// both BaseEntity and ISlugSupported. Because BaseMobEntity doesn't implement ISlugSupported, we can't use it now.
        /// TODO: Create a separate class BaseMobEntityWithUrl that extends ISlugSupported to use NopCommerce method instead of this one
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="SeName"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private string ValidateSeName(T entity, string SeName, string Name)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            //use name if sename is not specified
            if (String.IsNullOrWhiteSpace(SeName) && !String.IsNullOrWhiteSpace(Name))
                SeName = Name;

           
            //max length
            //For long URLs we can get the following error:
            //"the specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters"
            //that's why we limit it to 200 here (consider a store URL + probably added {0}-{1} below)
            SeName = CommonHelper.EnsureMaximumLength(SeName, 200);

           
            //ensure this sename is not reserved yet
            string entityName = typeof(T).Name;
            var seoSettings = EngineContext.Current.Resolve<SeoSettings>();
            int i = 2;
            var tempSeName = SeName;
            while (true)
            {
                //check whether such slug already exists (and that is not the current entity)
                var urlRecord = _urlRecordService.GetBySlug(tempSeName);
                var reserved1 = urlRecord != null && !(urlRecord.EntityId == entity.Id && urlRecord.EntityName.Equals(entityName, StringComparison.InvariantCultureIgnoreCase));
                //and it's not in the list of reserved slugs
                var reserved2 = seoSettings.ReservedUrlRecordSlugs.Contains(tempSeName, StringComparer.InvariantCultureIgnoreCase);
                if (!reserved1 && !reserved2)
                    break;

                tempSeName = string.Format("{0}-{1}", SeName, i);
                i++;
            }
            SeName = tempSeName;

            return SeName;
        }

        private void InsertUrlRecord(T entity)
        {
            var namedEntity = (INameSupported)entity;
            var slugEntity = entity as ISlugSupported;

            var seName = SeoExtensions.GetSeName(namedEntity.Name, true, false);
            seName = ValidateSeName(entity, seName, namedEntity.Name);
            var urlRecord = new UrlRecord() {
                EntityId = entity.Id,
                EntityName = typeof(T).Name,
                IsActive = true,
                LanguageId = _workContext.WorkingLanguage.Id,
                Slug = seName
            };

            _urlRecordService.InsertUrlRecord(urlRecord);
        }

        private void UpdateUrlRecord(T entity, string currentSlug)
        {
            var urlRecord = _urlRecordService.GetBySlug(currentSlug);
            var namedEntity = (INameSupported)entity;
            var newSlug = Nop.Services.Seo.SeoExtensions.GetSeName(namedEntity.Name, true, false);

            newSlug = ValidateSeName(entity, newSlug, namedEntity.Name);
            urlRecord.EntityName = typeof(T).Name;
            urlRecord.LanguageId = _workContext.WorkingLanguage.Id;
            urlRecord.Slug = newSlug;

            _urlRecordService.UpdateUrlRecord(urlRecord);
        }
        #endregion
        
    }
}
