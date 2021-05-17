using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Data.SqlClient;
using ServiceStack.Redis;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebApplication1.Filter;
using WebApplication1.Middle;
using BLL.Model;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.Extensions.FileProviders;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(option =>
                option.Filters.Add(typeof(ContentFilter))
            );
            services.AddAutoMapper(typeof(AutoMapperConfigs));
            services.AddCors(option=>
                option.AddDefaultPolicy(builder=>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                })
             );
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<SqlConnection>(services =>
            {
                SqlConnection con = new SqlConnection(Configuration.GetConnectionString("SqlServerConnectionStr"));
                return con;
            });
            services.AddScoped<RedisManagerPool>(redisManagerPool =>
            {
                return new RedisManagerPool(Configuration.GetConnectionString("RedisConnectionStr"));
            });
            services.AddScoped<CurrentUser>(services=>
            {
                var context=services.GetService<IHttpContextAccessor>();
                var allowAnonymous=context.HttpContext?.GetEndpoint()?.Metadata?.GetMetadata<AllowAnonymousAttribute>();
                if (allowAnonymous != null)
                {
                    return new CurrentUser();
                }
                AuthenticationHeaderValue.TryParse(context.HttpContext.Request.Headers["Authorization"].ToString(), out AuthenticationHeaderValue headerValue);
                var token = headerValue?.Parameter;
                if(string.IsNullOrWhiteSpace(token))
                {
                    throw new Exception("401");
                }
                var redisManager = services.GetService<RedisManagerPool>();
                using(var redisClient = redisManager.GetClient())
                {
                    redisClient.Db = (int)RedisDB.DB0;
                    var userCode=redisClient.Get<string>(token);
                    if (userCode == null)
                    {
                        throw new Exception("401");
                    }
                    var currentUser = redisClient.Get<CurrentUser>(userCode);
                    return currentUser;
                }
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "KaKa API"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                var securityRequirement = new OpenApiSecurityRequirement
                {
                   {
                       new OpenApiSecurityScheme
                       {
                           Reference = new OpenApiReference
                           {
                              Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                           }
                       },
                       new string[] {}
                    }
                 };
                c.AddSecurityRequirement(securityRequirement);
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var modelPath = Path.Combine(AppContext.BaseDirectory, "BLL.Model.xml");
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(modelPath);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseCors();
            app.UseExceptionHandle<ErrorMiddle>();
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseTokenAuthentication<TokenMidele>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public void ConfigureContainer(ContainerBuilder container)
        {
            container.RegisterAssemblyTypes(Assembly.Load("BLL.Logic"))
              .AsImplementedInterfaces();
            container.RegisterAssemblyTypes(Assembly.Load("DAL.Sql"))
              .AsImplementedInterfaces();
        }
    }
}
