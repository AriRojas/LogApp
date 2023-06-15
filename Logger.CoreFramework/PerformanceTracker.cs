using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Logger.CoreFramework
{
    public class PerformanceTracker
    {
        private readonly Stopwatch _stopwatch;
        private readonly LogDetail _logDetail;

        public PerformanceTracker(string name, string userId, string userName, string location, string product, string layer)
        {
            _stopwatch = Stopwatch.StartNew();
            _logDetail = new LogDetail()
            {
                Message = name,
                UserId = userId,
                UserName = userName,
                Product = product,
                Layer = layer,
                Location = location,
                Hostname = Environment.MachineName
            };

            var beginTime = DateTime.Now;
            _logDetail.AdditionalInformation = new Dictionary<string, object>()
            {
                {"Started", beginTime.ToString(CultureInfo.InvariantCulture)}
            };
        }

        public PerformanceTracker(string name, string userId, string userName, string location, string product, string layer,
            Dictionary<string, object> perfParams)
            : this(name, userId, userName, location, product, layer)
        {
            foreach (var item in perfParams)
            {
                _logDetail.AdditionalInformation.Add($"input-{item.Key}", item.Value);
            }
        }

        public void Stop()
        {
            _stopwatch.Stop();
            _logDetail.ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            Logger.LogPerformance(_logDetail);
        }
    }
}