using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Media;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mob.Core.Domain
{
    public abstract class BaseMobPictureEntity: BaseMobEntity
    {
        public int PictureId { get; set; }
        public int EntityId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
