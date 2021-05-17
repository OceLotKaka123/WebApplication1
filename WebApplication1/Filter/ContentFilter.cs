using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Filter
{
    /// <summary>
    /// 内容过滤
    /// </summary>
    public class ContentFilter:ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if(context.Result is BadRequestObjectResult)
            {
                var objectResult = context.Result as BadRequestObjectResult;
                context.Result = new BadRequestObjectResult(new ApiResultModel
                {
                    Code = 400,
                    Content = ((ValidationProblemDetails)objectResult.Value).Errors.Values.First().GetValue(0),
                    Message = null
                });
            }
            else if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                context.Result = new OkObjectResult(new ApiResultModel
                {
                    Code = 200,
                    Content = objectResult.Value,
                    Message = null
                });
            }
            base.OnResultExecuting(context);
        }
    }
}
