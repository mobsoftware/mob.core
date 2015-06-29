using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mob.Core.Domain
{
    public abstract class BaseMobEntity : BaseEntity
    {
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
