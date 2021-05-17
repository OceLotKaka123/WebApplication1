using BLL.Model;
using Commons;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApplication1.Middle
{
    public class TokenMidele
    {
        private readonly RequestDelegate _next;
        private readonly RedisManagerPool _redisManagerPool;
        public TokenMidele(RequestDelegate next,RedisManagerPool redisManagerPool)
        {
            _next = next;
            _redisManagerPool = redisManagerPool;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.GetEndpoint() == null)
            {
                await _next(context);
            }
            else
            {
                var isAllow = context.GetEndpoint().Metadata?.GetMetadata<AllowAnonymousAttribute>();
                if (isAllow == null)
                {
                    using (var redisClient = _redisManagerPool.GetClient())
                    {
                        redisClient.Db = (int)RedisDB.DB0;
                        AuthenticationHeaderValue.TryParse(context.Request.Headers["Authorization"].ToString(), out AuthenticationHeaderValue headerValue);
                        var token = headerValue?.Parameter;
                        if (String.IsNullOrWhiteSpace(token))
                        {
                            context.Response.StatusCode = 401;
                            throw new CNException("token信息错误");
                        }
                        var userCode = redisClient.Get<string>(token);
                        if (userCode == null)
                        {
                            context.Response.StatusCode = 401;
                            throw new CNException("token信息错误");
                        }
                    }
                }
            }
            await _next(context);
        }
    }
}
