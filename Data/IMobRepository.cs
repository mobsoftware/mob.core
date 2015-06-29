using Nop.Core;
using Nop.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mob.Core.Data
{
    public interface IMobRepository<T> : IRepository<T> where T: BaseEntity
    {

    }
}
