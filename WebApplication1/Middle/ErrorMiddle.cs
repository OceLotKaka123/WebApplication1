using Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApplication1.Filter;

namespace WebApplication1.Middle
{
    public class ErrorMiddle
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorMiddle> _logger;
        public ErrorMiddle(RequestDelegate next,ILogger<ErrorMiddle> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is CNException)
                {
                    await HandleExceptionAsync(context, ex);
                }
                else if (ex is DbException)
                {
                    _logger.LogError(@$"{context.Request.Path}\n{ex.Message}\n 
                        {ex.StackTrace.Trim()}");
                    await context.Response.WriteAsJsonAsync(new ApiResultModel
                    {
                        Code = 204,
                        Message = "数据库执行错误",
                        Content = null
                    });
                }
                else
                {
                    _logger.LogError(@$"{context.Request.Path}\n{ex.Message}\n 
                        {ex.StackTrace.Trim()}");
                    await context.Response.WriteAsJsonAsync(new ApiResultModel
                    {
                        Code = 204,
                        Message = "服务器内部错误",
                        Content = null
                    });
                }
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.StatusCode == 401)
            {
                return context.Response.WriteAsJsonAsync(new ApiResultModel
                {
                    Code = 401,
                    Message = "token信息错误",
                    Content = null
                });
            }
            //密码错误渲染前端
            if (context.Response.StatusCode == 600)
            {
                return context.Response.WriteAsJsonAsync(new ApiResultModel
                {
                    Code = 600,
                    Message = ex.Message,
                    Content = null
                });
            }
            //后台抛出的问题(自定义)
            context.Response.StatusCode = 500;
            return context.Response.WriteAsJsonAsync(new ApiResultModel { 
                Code=500,
                Message=ex.Message,
                Content=null
            });
        }
    }
}
