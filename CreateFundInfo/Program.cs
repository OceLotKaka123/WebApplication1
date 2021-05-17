using Dapper;
using Entities;
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
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CreateFundInfo
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
            services.AddScoped<RedisManagerPool>(redis =>
            {
                return new RedisManagerPool(Configuration.GetConnectionString("RedisConnectionStr"));
            });
            var buildService = services.BuildServiceProvider();
            SqlConnection con = buildService.GetService<SqlConnection>();
            RedisManagerPool redisManagerPool = buildService.GetService<RedisManagerPool>();
            CreateCodeInfo(con);
            CreateFundTask(con);
        }
        /// <summary>
        /// 爬取基金编码
        /// </summary>
        /// <param name="con"></param>
        private static void CreateCodeInfo(SqlConnection con)
        {
            WebClient wc = new WebClient();
            string html = wc.DownloadString("http://fund.eastmoney.com/allfund.html");
            var matches = Regex.Matches(html, "<a href=\"http://fund.eastmoney.com/(......).html\">");
            foreach (Match match in matches)
            {
                con.Execute("insert into FundCode(Code) values(@Code)", new { Code = match.Groups[1].Value });
            }
        }
        private static void CreateFundTask(SqlConnection con)
        {
            var fundCodes = con.Query<string>(@"select Code from FundCode");
            foreach (var fundCode in fundCodes)
            {
                Task.Run(() =>
                {
                    string html;
                    string src = $"http://fund.eastmoney.com/{fundCode}.html";
                    WebClient wc = new WebClient();
                    wc.Dispose();
                    html = wc.DownloadString(src);
                    CreateFundInfo(fundCode, html);
                });
            }
        }
        private static void CreateFundInfo(string fundCode,string html)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = "server=DESKTOP-QOA1RRD;user=sa;password=156160;Database=Fund";
                BaseFundEntity baseFund = new BaseFundEntity();
                FundHistoryInfoEntity fundHistoryInfo = new FundHistoryInfoEntity();
                baseFund.FundCode = fundCode;

                ///基金规模
                string fundScale_str = $"<td><a href=\"http://fundf10.eastmoney.com/gmbd_(......).html\">基金规模</a>：({Dot.dotNum})亿元（(....-..-..)）</td>";
                var fundScale_data = Regex.Match(html, fundScale_str);
                var fundScaleMoney = fundScale_data.Groups[2].Value;
                fundHistoryInfo.FundScale = fundScaleMoney;
                var fundScaleDate = fundScale_data.Groups[3].Value;
                fundHistoryInfo.FundScaleDate = GetDatetime(fundScaleDate);
                ///管理人
                var manager_str = $"<td><span class=\"letterSpace01\">管 理 人</span>：<a href=\"http://fund.eastmoney.com/company/(........).html\">({Dot.dotNum})</a></td>";
                var manager = Regex.Match(html, manager_str).Groups[2].Value;
                fundHistoryInfo.Manager = manager;
                //成立日
                var foundDate_str = "<td><span class=\"letterSpace01\">成 立 日</span>：(....-..-..)</td>";
                var foundDate = Regex.Match(html, foundDate_str).Groups[1].Value;
                fundHistoryInfo.FoundDate = GetDatetime(foundDate);
                //基金类型
                var foudType_str = "<td>基金类型：<a href=\"http://fund.eastmoney.com/(..|...|....)_jzzzl.html#os_0;isall_0;ft_;pt_(.|..)\">(...|....)</a>((&nbsp;&nbsp;(.)&nbsp;&nbsp;(...|....))|)<";
                var foudType_Match = Regex.Match(html, foudType_str);
                var foudType = foudType_Match.Groups[3].Value;
                fundHistoryInfo.FundType = foudType;
                var risk = foudType_Match.Groups[7].Value;
                fundHistoryInfo.FundRisk = risk;
                //项目经理
                string Direct_str = "<a href=\"http://fundf10.eastmoney.com/jjjl_(......).html\">(....|...|..)</a>";
                var Direct_Match = Regex.Match(html, Direct_str);

                var Direact = Direct_Match.Groups[2].Value;
                fundHistoryInfo.Director = Direact;
                //以正则表达式的形式匹配到字符串网页中想要的数据
                var fundName = Regex.Match(html, "<div class=\"fundDetail-tit\"><div style=\"float: left\">(" + Dot.dotNum + ")<span>");
                baseFund.FundName = fundName.Groups[1].Value;
                var matches = Regex.Match(html, @"<dd><span>(.*)</span></dd>");
                var c = "<span class=\"ui-font-large  ui-num\" id=\"gz_gsz\">(..)</span>";
                //净值估算
                var shamValue = Regex.Match(html, c);
                baseFund.ShamValue = shamValue.Groups[1].Value;
                var h = "<span class=\"ui-font-middle (ui-color-red|ui-color-green|) ui-num\">" + "(..|...%|....%|.....%|......%|.......%)</span>";
                var s = "<span class=\"ui-font-large (ui-color-red|ui-color-green|) ui-num\">(......|..)</span>";

                //增长数据
                var d = Regex.Matches(matches.Value, h);
                //单位、累计净值
                var d1 = Regex.Matches(matches.Value, s);
                if (d1.Count < 2 || d.Count < 7)
                {
                    return;
                }
                baseFund.TotalValue = d1[1].Groups[2].Value;
                baseFund.RealValue = d1[0].Groups[2].Value;
                baseFund.RealPercentValue = d[2].Groups[2].Value;
                fundHistoryInfo.OneMonthValue = d[0].Groups[2].Value;
                fundHistoryInfo.ThreeMonthValue = d[3].Groups[2].Value;
                fundHistoryInfo.SixMonthValue = d[5].Groups[2].Value;
                fundHistoryInfo.OneYear = d[1].Groups[2].Value;
                fundHistoryInfo.ThreeYear = d[4].Groups[2].Value;
                fundHistoryInfo.FoundValue = d[6].Groups[2].Value;

                int fundId = con.QueryFirst<int>(@"insert into BaseFund(RealValue,RealPercentValue,ShamValue,TotalValue,FundName,FundCode) values(@RealValue,@RealPercentValue,@ShamValue,@TotalValue,@FundName,@FundCode) select Id from BaseFund where FundCode=@FundCode", baseFund);
                fundHistoryInfo.FundId = fundId;
                con.Execute("insert into FundHistoryInfo(FundId,OneMonthValue,ThreeMonthValue,SixMonthValue,OneYear,ThreeYear,FoundValue,FundType,FundRisk,FundScale,FundScaleDate,Director,FoundDate,Manager) values (@FundId,@OneMonthValue,@ThreeMonthValue,@SixMonthValue,@OneYear,@ThreeYear,@FoundValue,@FundType,@FundRisk,@FundScale,@FundScaleDate,@Director,@FoundDate,@Manager)", fundHistoryInfo);
            }
        }
        private static DateTime? GetDatetime(string value)
        {
            if(DateTime.TryParse(value, out DateTime dateValue))
            {
                return dateValue;
            }
            return null;
        }
    }
}
