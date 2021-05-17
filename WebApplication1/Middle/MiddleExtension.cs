using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Middle
{
    public static class MiddleExtension
    {
        public static void UseExceptionHandle<T>(this IApplicationBuilder app)
        {
            app.UseMiddleware<T>();
        }
        public static void UseTokenAuthentication<T>(this IApplicationBuilder app)
        {
            app.UseMiddleware<T>();
        }
    }
}
