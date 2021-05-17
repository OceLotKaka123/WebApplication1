using BLL.Model;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InitData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            var Configuration = builder.Build();
            ServiceCollection services = new ServiceCollection();
            services.AddScoped<SqlConnection>(con=>
            {
                return new SqlConnection(Configuration.GetConnectionString("SqlServerConnectionStr"));
            });
            services.AddScoped<RedisManagerPool>(redis=>
            {
                return new RedisManagerPool(Configuration.GetConnectionString("RedisConnectionStr"));
            });
            var serviceProvider=services.BuildServiceProvider();
            SqlConnection con = serviceProvider.GetService<SqlConnection>();
            RedisManagerPool redisManagerPool = serviceProvider.GetService<RedisManagerPool>();
            using(var redisManager = redisManagerPool.GetClient())
            {
                //FundAnswer fundAnswer = new FundAnswer
                //{
                //    AnswerText = "那个基金增长最高",
                //    AnswerNumber = 10001
                //};
                redisManager.Db = (int)RedisDB.DB4;
                var fundTypeRedis = con.Query<FundTypeRedisModel>(@"select FundType,COUNT(FundType) FundTypeValue from FundHistoryInfo group by FundType");
                var redisTyped = redisManager.As<FundTypeRedisModel>();
                redisTyped.StoreAll(fundTypeRedis);
                redisManager.Db = (int)RedisDB.DB0;
                //var redisUserInfoModel = redisManager.As<UserInfoRedisModel>();
                //var allUserInfo = con.Query<UserInfoRedisModel>(@"select 
                //   Id,Name,UserCode,TelPhone from UserInfo where Valid=1");
                //redisUserInfoModel.StoreAll(allUserInfo);
                redisManager.Db = (int)RedisDB.DB4;
                var redisBaseFund = redisManager.As<FundRedisModel>();
                var allFunds = con.Query<FundRedisModel>(@"select Id,
                   FundCode,FundName,RealValue,RealPercentValue,ShamValue,
                   TotalValue,DateNow from BaseFund");
                redisBaseFund.StoreAll(allFunds);
            }
        }
    }
}
