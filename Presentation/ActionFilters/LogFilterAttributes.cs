using Entities1.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Services.Contracts;

namespace Presentation.ActionFilters
{
    public class LogFilterAttributes : ActionFilterAttribute
    {
        private readonly ILoggerService _logger;

        public LogFilterAttributes(ILoggerService logger)
        {
            _logger=logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInfo(Log("OnActionExecuting", context.RouteData));
        }

        private string Log(string modelName, RouteData routeData)
        {
            var logDetails = new LogDetails()
            {
                ModelName = modelName,
                Controller = routeData.Values["controller"],
                Action=routeData.Values["action"]

            };

            if (routeData.Values.Count >= 3) 
            {
                logDetails.Id = routeData.Values["id"];
            }
            return logDetails.ToString();
        }
    }
}
