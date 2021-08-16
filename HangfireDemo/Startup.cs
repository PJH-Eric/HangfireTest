using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.AttributeFilters;
using Dapper.FluentMap;
using Hangfire;
using Hangfire.MySql;
using HangfireDemo.Domain;
using HangfireDemo.Filter;
using HangfireDemo.Job;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelDTO.FluentMapper;
using MySql.Data.MySqlClient;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace HangfireDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppManager.AppSetting = configuration.Get<AppSetting>();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbConnection>(db => new MySqlConnection(
                    Configuration.GetConnectionString("BackstageConnectionString")));         

            services.AddHangfire(config => config
                            .UseStorage(new MySqlStorage(
                            Configuration.GetConnectionString("HangfireConnectionString"),
                            new MySqlStorageOptions
                            {
                                TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, 
                                QueuePollInterval = TimeSpan.FromSeconds(15),            
                                JobExpirationCheckInterval = TimeSpan.FromHours(1),      
                                CountersAggregateInterval = TimeSpan.FromMinutes(5),      
                                PrepareSchemaIfNecessary = true,                          
                                DashboardJobListLimit = 50000,                            
                                TransactionTimeout = TimeSpan.FromMinutes(1),            
                                TablesPrefix = "Hangfire."                                  
                            }
                            )));
            services.AddHangfireServer();
            services.AddControllers()
                .AddControllersAsServices();

            FluentMapperInit.Init();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            IEnumerable<Assembly> assemblies = new List<Assembly>()
            {
                Assembly.Load("Service"),
                Assembly.Load("Repository"),
                Assembly.Load("ModelDTO"),
            };

            foreach (var assembly in assemblies)
            {
                builder
                .RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InstancePerLifetimeScope()
                .WithAttributeFiltering();
            }

            builder.RegisterType<MattermostMessage>().As<IMessageService>().WithParameter("Url", AppManager.AppSetting.hook_url);
            // 注入使用者身份
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().PropertiesAutowired().SingleInstance();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 加入Hangfire伺服器
            app.UseHangfireServer();

            // 加入Hangfire控制面板
            app.UseHangfireDashboard(
                pathMatch: "/hangfire",
                options: new DashboardOptions()
                { // 使用自訂的認證過濾器
                    Authorization = new[] { new DashboardAuthorizationFilter() }
                }
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();

            });
            BackgroundJob.Enqueue<DummyStaticJob>(x => x.Run());
        }
    }
}
