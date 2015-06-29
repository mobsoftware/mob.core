using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Domain;
using Nop.Core;
using Nop.Data.Mapping;

namespace Mob.Core.Data
{
    public class BaseMobEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : BaseMobEntity
    {
        public BaseMobEntityTypeConfiguration()
        {
            ToTable(typeof (T).Name);
            HasKey(x => x.Id);
            Property(x => x.DateCreated);
            Property(x => x.DateUpdated);
        } 
    }
}
