
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using opensmsplatform.Common;
using opensmsplatform.Common.Core;
using opensmsplatform.Common.HttpContextUser;
using opensmsplatform.Extension;
using opensmsplatform.Extensions;
using Serilog;
using System.Collections.Generic;

namespace opensmsplatform
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

            var app = builder.Build();
            app.ConfigureApplication();    //�� Service
            app.UseApplicationSetup();

            //�رռ�¼Post������־
            app.UseMiddleware<PostLogMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
