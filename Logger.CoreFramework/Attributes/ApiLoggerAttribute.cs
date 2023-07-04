using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Logger.CoreFramework.Attributes
{
    public class ApiLoggerAttribute : ActionFilterAttribute
    {
        private string _productName;

        public ApiLoggerAttribute(string productName)
        {
            _productName = productName;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var user = actionContext.RequestContext.Principal as ClaimsPrincipal;
            var additionalInfo = new Dictionary<string, object>();
            string userId, userName;
            Helpers.GetUserData(additionalInfo, user, out userId, out userName);

            string location;
            Helpers.GetLocationForApiCall(actionContext.RequestContext, additionalInfo, out location);

            var qs = actionContext.Request.GetQueryNameValuePairs()
                .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);

            var i = 0;
            foreach ( var kv in qs )
            {
                var newKey = string.Format($"q-{i++}-{kv.Key}");
                if(!additionalInfo.ContainsKey(newKey))
                    additionalInfo.Add(newKey, kv.Value);
            }

            var referrer = actionContext.Request.Headers.Referrer;
            if(referrer != null)
            {
                var source = actionContext.Request.Headers.Referrer
            }
            var stopwatch = new PerformanceTracker(perfName, userId, userName, location, _productName, _layerName, additionalInfo);
            filterContext.HttpContext.Items["Stopwatch"] = stopwatch;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
        }
    }
}