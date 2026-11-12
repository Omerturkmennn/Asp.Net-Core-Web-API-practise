using Entities1.Exceptions1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ValidateMediatypeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderPresent=context.HttpContext.Request.Headers.ContainsKey("Accept");

            if (!acceptHeaderPresent) 
            {
                context.Result = new BadRequestObjectResult($"Accept header is missing!");
                return;
            }

            var mediaType = context.HttpContext.Response.Headers["Accept"].FirstOrDefault();

            if (MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = new BadRequestObjectResult($"Media type not present." +
                    $"Please add accept header with required media type");
                return;
            }

            context.HttpContext.Items.Add("AcceptHeaderMediaTyoe", outMediaType);
        }
    }
}
