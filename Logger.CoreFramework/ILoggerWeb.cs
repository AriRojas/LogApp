using System;

namespace Logger.CoreFramework
{
    public interface ILoggerWeb
    {
        void LogError();

        void StartLogPerformance();

        void StopLogPerformance();
    }

    public class LoggerWeb : ILoggerWeb
    {
        public void LogError()
        {
            throw new NotImplementedException();
        }

        public void StartLogPerformance()
        {
            throw new NotImplementedException();
        }

        public void StopLogPerformance()
        {
            throw new NotImplementedException();
        }
    }

    public class LoggerWebApi : ILoggerWeb
    {
        public void LogError()
        {
            throw new NotImplementedException();
        }

        public void StartLogPerformance()
        {
            throw new NotImplementedException();
        }

        public void StopLogPerformance()
        {
            throw new NotImplementedException();
        }
    }
}