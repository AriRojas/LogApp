using System.Web.Mvc;

namespace Logger.CoreFramework.Attributes
{
    public class TrackPerformanceMvcAppAttribute : ActionFilterAttribute
    {
        private string _productName;
        private string _layerName;

        public TrackPerformanceMvcAppAttribute(string productName, string layerName)
        {
            _productName = productName;
            _layerName = layerName;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userId, userName, location;
            var additionalInfo = Helpers.GetWebLogData(out userId, out userName, out location);
            var type = filterContext.HttpContext.Request.RequestType;
            var perfName = $"{filterContext.ActionDescriptor.ActionName}_{type}";
            var stopwatch = new PerformanceTracker(perfName, userId, userName, location, _productName, _layerName, additionalInfo);

            filterContext.HttpContext.Items["Stopwatch"] = stopwatch;
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var stopwatch = filterContext.HttpContext.Items["Stopwatch"] as PerformanceTracker;
            if(stopwatch != null) 
                stopwatch.Stop();

        }
    }
}
