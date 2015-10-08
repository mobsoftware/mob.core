using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mob.Core.Data;

namespace Mob.Core.Migrations
{
    public abstract class MobMigrationConfiguration<T> : DbMigrationsConfiguration<T> where T : MobDbContext
    {

    }
}
