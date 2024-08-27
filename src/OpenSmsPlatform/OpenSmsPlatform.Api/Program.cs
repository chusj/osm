
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
            //����һ��������
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

            //ӳ��
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();

            //����
            builder.Services.AddSingleton(new AppSettings(builder.Configuration));
            builder.Services.AddAllOptionRegister();

            //����
            builder.Services.AddCacheSetup();

            //ORM
            builder.Services.AddSqlSugarSetup();

            //Http����������
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //ע��������ڻ�ȡjwt���û���Ϣ
            builder.Services.AddScoped<IUser, AspNetUser>();

            // serilog
            var loggerConfiguration = new LoggerConfiguration()
                                    .ReadFrom.Configuration(AppSettings.Configuration)
                                    .Enrich.FromLogContext()
                                    //.WriteTo.Console()
                                    //.WriteTo.File(Path.Combine("Logs", "Api.seriLog.txt"));
                                    //���������̨
                                    //.WriteToConsole()
                                    //����־���浽�ļ���
                                    .WriteToFile();

            Log.Logger = loggerConfiguration.CreateLogger();
            builder.Host.UseSerilog();

            builder.Services.AddHttpClient();


            // ���� Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "OpenSmsPlatform", Version = "v1" });

                // ����ProjectName.xml
                var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
                options.IncludeXmlComments(apiXmlPath);

                // ����ProjectName.Model.xml
                var modelXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}".Replace(".Api", ".Model.xml");
                var modelXmlPath = Path.Combine(AppContext.BaseDirectory, modelXmlFile);
                options.IncludeXmlComments(modelXmlPath);
            });


            var app = builder.Build();
            app.ConfigureApplication();    //�� Service
            app.UseApplicationSetup();

            //�رռ�¼Post������־
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
