using Nop.Core;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mob.Core.Data
{
    public class MobRepository<T> : EfRepository<T>, IMobRepository<T> where T: BaseEntity
    {
        public MobRepository(IDbContext context)
            :base(context)
        {

        }
    }
}
