using Autofac;
using Mob.Core.Data;
using Mob.Core.Services;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Mob.Core.UI;
using Nop.Web.Framework.UI;
using Nop.Core.Configuration;

namespace Mob.Core
{
    public abstract class  BaseMobDependencyRegistrar: IDependencyRegistrar
    {
        //the context name
        public abstract string ContextName { get; }

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {


            var asm = AppDomain.CurrentDomain.GetAssemblies();


            builder.RegisterGeneric(typeof(BaseEntityService<>)).As(typeof(IBaseEntityService<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseEntityWithPictureService<,>)).As(typeof(IBaseEntityWithPictureService<,>)).InstancePerLifetimeScope();

            builder.RegisterType<MobPageHeadBuilder>().As<MobPageHeadBuilder>().InstancePerRequest();

            //register all the implemented services in various mob plugins
            builder.RegisterAssemblyTypes(asm).AsClosedTypesOf(typeof(BaseEntityService<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(asm).AsClosedTypesOf(typeof(BaseEntityWithPictureService<,>))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            //register all the repositories
            builder.RegisterGeneric(typeof(MobRepository<>)).As(typeof(IMobRepository<>))
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                        .InstancePerRequest();

        }

        public virtual int Order
        {
            get { return 0; }
        }
    }
}
