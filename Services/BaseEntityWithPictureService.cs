using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Seo;
using Mob.Core.Domain;
using Nop.Services.Seo;
using Mob.Core.Data;
using Nop.Services.Media;

namespace Mob.Core.Services
{
    /// <summary>
    /// Generic base service to standardize Service APIs
    /// </summary>
    public abstract class BaseEntityWithPictureService<T, P> : BaseEntityService<T>, IBaseEntityWithPictureService<T, P>
        where T : BaseMobEntity
        where P : BaseMobPictureEntity
    {

        private readonly IMobRepository<T> _repository;
        private readonly IMobRepository<P> _pictureRepository;
        private readonly IWorkContext _workContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IPictureService _pictureService;


        public BaseEntityWithPictureService(IMobRepository<T> repository, IMobRepository<P> pictureRepository, IPictureService pictureService)
            : base(repository)
        {
            _repository = repository;
            _pictureRepository = pictureRepository;
            _pictureService = pictureService;
        }

        public BaseEntityWithPictureService(IMobRepository<T> repository, IMobRepository<P> pictureRepository, IPictureService pictureService, IWorkContext workContext, IUrlRecordService urlRecordService)
            : this(repository, pictureRepository, pictureService)
        {
            _workContext = workContext;
            _urlRecordService = urlRecordService;
        }

       

        #region Main Entity Operations

        public new void Insert(T entity)
        {
            base.Insert(entity);

            if (entity is ISlugSupported && entity is INameSupported)
                InsertUrlRecord(entity);

        }

        public new void Update(T entity)
        {

            base.Update(entity);

            if (entity is ISlugSupported && entity is INameSupported)
            {
                var currentSlug = _urlRecordService.GetActiveSlug(entity.Id, typeof(T).Name, _workContext.WorkingLanguage.Id);

                if (!string.IsNullOrEmpty(currentSlug))
                    UpdateUrlRecord(entity, currentSlug);
                else
                    InsertUrlRecord(entity);
            }


        }
        public new void Delete(T entity)
        {
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

           base.Delete(entity);

        }

        public new T GetById(int id)
        {
            return base.GetById(id);
        }
        public new List<T> GetAll()
        {
            return base.GetAll();
        }

        

        #endregion


        #region Entity Pictures

        public void InsertPicture(P entity)
        {
            _pictureRepository.Insert(entity);
        }
        public void UpdatePicture(P entity)
        {
            _pictureRepository.Update(entity);
        }
        public void DeletePicture(P entity)
        {
            _pictureRepository.Delete(entity);
        }
        /// <summary>
        /// Gets the Entity Picture record for the given id.
        /// </summary>
        /// <param name="id">Id of the entitypicture record.</param>
        public P GetPictureById(int id)
        {
            return _pictureRepository.GetById(id);
        }

        /// <summary>
        /// Gets all pictures for the specified entity
        /// </summary>
        /// <returns></returns>
        public List<P> GetAllPictures(int entityId)
        {
           return _pictureRepository.Table.Where(x => x.EntityId == entityId).ToList();
        }

        /// <summary>
        /// Gets the first entity picture for the specified entity from EntityPicture table
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public P GetFirstEntityPicture(int entityId)
        {
            return GetAllPictures(entityId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first picture for the specified entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Picture GetFirstPicture(int entityId)
        {
            var ep = GetFirstEntityPicture(entityId);
            
            return _pictureService.GetPictureById(ep.PictureId);
        }

        #endregion



        #region SortableOperations
        public void UpdateDisplayOrder(int id, int newDisplayOrder)
        {
            var entity = GetById(id);

            var sortableEntity = (ISortableSupported)entity;

            var oldDisplayOrder = sortableEntity.DisplayOrder;

            var items = _repository.Table
                .Where(x => ((ISortableSupported)x).DisplayOrder >= oldDisplayOrder)
                .ToList();

            sortableEntity.DisplayOrder = newDisplayOrder;

        }
        #endregion


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
