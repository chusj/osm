using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace OpenSmsPlatform.Extensions
{
    /// <summary>
    /// Autofac 属性注册
    /// </summary>
    public class AufofacPropertityModuleReg : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
        }
    }
}
