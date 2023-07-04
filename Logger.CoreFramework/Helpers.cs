using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Web;

namespace Logger.CoreFramework
{
    public class Helpers
    {
        #region Public Methods

        public static Dictionary<string, object> GetWebLogData(out string userId, out string userName, out string location)
        {
            var additionalInfo = new Dictionary<string, object>();
            GetRequestData(additionalInfo, out location);
            GetUserData(additionalInfo, out userId, out userName);
            GetSessionData(additionalInfo);

            return additionalInfo;
        }

        public static void LogWebUsage(string product, string layer, string activityName, Dictionary<string, object> additionalInfo = null)
        {
            string userId, userName, location;
            var webInfo = GetWebLogData(out userId, out userName, out location);

            if (additionalInfo != null)
            {
                foreach (var item in additionalInfo.Keys)
                {
                    webInfo.Add($"Info-{item}", additionalInfo[item]);
                }
            }
            var usageInformation = new LogDetail()
            {
                Product = product,
                Layer = layer,
                Location = location,
                UserId = userId,
                UserName = userName,
                Hostname = Environment.MachineName,
                CorrelationId = HttpContext.Current.Session.SessionID,
                Message = activityName,
                AdditionalInformation = webInfo
            };

            Logger.LogUsage(usageInformation);
        }

        public static void LogWebDiagnostics(string product, string layer, string message, Dictionary<string, object> additionalInfo)
        {
            var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnabelDiagnostics"]);
            if (!writeDiagnostics)
                return;

            string userId, userName, location;
            var webInfo = GetWebLogData(out userId, out userName, out location);

            if (additionalInfo != null)
            {
                foreach (var item in additionalInfo.Keys)
                {
                    webInfo.Add($"Info-{item}", additionalInfo[item]);
                }
            }

            var diagnosticInfo = new LogDetail()
            {
                Product = product,
                Layer = layer,
                Location = location,
                UserId = userId,
                UserName = userName,
                Hostname = Environment.MachineName,
                CorrelationId = HttpContext.Current.Session.SessionID,
                Message = message,
                AdditionalInformation = webInfo
            };

            Logger.LogDiagnostics(diagnosticInfo);
        }

        public static void LogWebError(string product, string layer, Exception ex)
        {
            string userId, userName, location;
            var webInfo = GetWebLogData(out userId, out userName, out location);

            var errorInformation = new LogDetail()
            {
                Product = product,
                Layer = layer,
                Location = location,
                UserId = userId,
                UserName = userName,
                Hostname = Environment.MachineName,
                CorrelationId = HttpContext.Current.Session.SessionID,
                Exception = ex,
                AdditionalInformation = webInfo
            };

            Logger.LogError(errorInformation);
        }

        #endregion Public Methods

        #region Private Methods

        private static void GetRequestData(Dictionary<string, object> additionalInfo, out string location)
        {
            location = string.Empty;
            var request = HttpContext.Request;
            if (request != null)
            {
                location = request.Path;
                string type, version;
                GetBrowserInfo(request, out type, out version);
                additionalInfo.Add("Browser", $"{type}{version}");
                additionalInfo.Add("UserAgent", request.UserAgent);
                additionalInfo.Add("Languages", request.UserLanguages);

                foreach (var qsKey in request.QueryString.Keys)
                {
                    additionalInfo.Add($"QueryString-{qsKey}", request.QueryString[qsKey.ToString()]);
                }
            }
        }

        private static void GetBrowserInfo(HttpRequest request, out string type, out string version)
        {
            type = request.Browser.Type;
            if (type.StartsWith("Chrome") && type.Contains("Edge/"))
            {
                type = "Edge";
                version = "(v" + request.UserAgent.Substring(request.UserAgent.IndexOf("Edge/") + 5) + ")";
            }
            else
            {
                version = $"(v{request.Browser.MajorVersion}.{request.Browser.MinorVersion})";
            }
        }

        internal static void GetUserData(Dictionary<string, object> additionalInfo, out string userId, out string userName)
        {
            userId = string.Empty;
            userName = string.Empty;

            var user = ClaimsPrincipal.Current;
            if (user != null)
            {
                var i = 1; // to ensure uniqueness in the dictionary
                foreach (var claim in user.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                        userId = claim.Value;
                    else if (claim.Type == ClaimTypes.Name)
                        userName = claim.Value;
                    else
                        additionalInfo.Add($"UserClaim-{i++}{claim.Type}", claim.Value);
                }
            }
        }

        private static void GetSessionData(Dictionary<string, object> additionalInfo)
        {
            if (HttpContext.Current.Session != null)
            {
                foreach (var key in HttpContext.Current.Session.Keys)
                {
                    var keyName = key.ToString();
                    if (HttpContext.Current.Session[keyName] != null)
                    {
                        additionalInfo.Add($"Session-{keyName}", HttpContext.Current.Session[keyName]);
                    }
                }

                additionalInfo.Add("SessionId", HttpContext.Current.Session.SessionID);
            }
        }

        #endregion Private Methods
    }
}