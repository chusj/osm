
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using OpenSmsPlatform.Common;
using OpenSmsPlatform.Common.Core;
using OpenSmsPlatform.Common.HttpContextUser;
using OpenSmsPlatform.Extension;
using OpenSmsPlatform.Extensions;
using Serilog;
using System.Reflection;

namespace OpenSmsPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //创建一个构造器
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<AutofacModuleRegister>();
                    builder.RegisterModule<AufofacPropertityModuleReg>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    hostingContext.Configuration.ConfigureApplication();
                })
                ;

            builder.ConfigureApplication();

            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //映射
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();

            //配置
            builder.Services.AddSingleton(new AppSettings(builder.Configuration));
            builder.Services.AddAllOptionRegister();

            //缓存
            builder.Services.AddCacheSetup();

            //ORM
            builder.Services.AddSqlSugarSetup();

            //Http请求上下文
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //注册服务，用于获取jwt中用户信息
            builder.Services.AddScoped<IUser, AspNetUser>();

            // serilog
            var loggerConfiguration = new LoggerConfiguration()
                                    .ReadFrom.Configuration(AppSettings.Configuration)
                                    .Enrich.FromLogContext()
                                    //.WriteTo.Console()
                                    //.WriteTo.File(Path.Combine("Logs", "Api.seriLog.txt"));
                                    //输出到控制台
                                    //.WriteToConsole()
                                    //将日志保存到文件中
                                    .WriteToFile();

            Log.Logger = loggerConfiguration.CreateLogger();
            builder.Host.UseSerilog();

            builder.Services.AddHttpClient();


            // 配置 Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "OpenSmsPlatform", Version = "v1" });

                // 加载ProjectName.xml
                var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
                options.IncludeXmlComments(apiXmlPath);

                // 加载ProjectName.Model.xml
                var modelXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}".Replace(".Api", ".Model.xml");
                var modelXmlPath = Path.Combine(AppContext.BaseDirectory, modelXmlFile);
                options.IncludeXmlComments(modelXmlPath);
            });


            var app = builder.Build();
            app.ConfigureApplication();    //拿 Service
            app.UseApplicationSetup();

            //关闭记录Post请求日志
            app.UseMiddleware<PostLogMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenSmsPlatform V1");
                });
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
