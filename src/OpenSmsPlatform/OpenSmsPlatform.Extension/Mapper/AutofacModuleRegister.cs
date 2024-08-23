using Autofac;
using Autofac.Extras.DynamicProxy;
using OpenSmsPlatform.IService;
using OpenSmsPlatform.Repository;
using OpenSmsPlatform.Repository.UnitOfWorks;
using OpenSmsPlatform.Service;
using System.Reflection;

namespace OpenSmsPlatform.Extension
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            var servicesFile = Path.Combine(basePath, "opensmsplatform.Service.dll");
            var repositoryFile = Path.Combine(basePath, "opensmsplatform.Repository.dll");

            var aopTypes = new List<Type> { typeof(ServiceAOP) };
            builder.RegisterType<ServiceAOP>();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>))
                .InstancePerDependency(); //注册仓储
            builder.RegisterGeneric(typeof(BaseService<,>)).As(typeof(IBaseService<,>))
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(aopTypes.ToArray()); //注册服务


            //获取 Service.dll 程序集服务，并注册
            var assemblyServices = Assembly.LoadFrom(servicesFile);
            builder.RegisterAssemblyTypes(assemblyServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .EnableInterfaceInterceptors()
                .InterceptedBy(aopTypes.ToArray());

            //获取 Repository.dll 程序集服务，并注册
            var assemblyRepository = Assembly.LoadFrom(repositoryFile);
            builder.RegisterAssemblyTypes(assemblyRepository)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired();

            builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}
