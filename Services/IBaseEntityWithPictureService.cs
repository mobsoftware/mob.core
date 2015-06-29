using System.Collections.Generic;
using Mob.Core.Domain;
using Nop.Core.Domain.Media;

namespace Mob.Core.Services
{
    /// <summary>
    /// Generic service to standardize Service APIs
    /// </summary>
    public interface IBaseEntityWithPictureService<T, P> : IBaseEntityService<T> 
        where T : BaseMobEntity
        where P : BaseMobPictureEntity
    {

        void InsertPicture(P entity);
        void UpdatePicture(P entity);
        void DeletePicture(P entity);

        P GetPictureById(int id);


        List<P> GetAllPictures(int entityId);
        P GetFirstEntityPicture(int entityId);

        Picture GetFirstPicture(int entityId);


        void UpdateDisplayOrder(int id, int newDisplayOrder);


    }
}