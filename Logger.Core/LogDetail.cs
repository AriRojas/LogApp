namespace Logger.Core
{
    public class LogDetail
    {
        public DateTime TimeStamp { get; private set; }
        public string Message { get; set; }
        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string Hostname { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public long? ElapsedMilliseconds { get; set; }
        public Exception Exception { get; set; }
        public int CorrelationId { get; set; }
        public Dictionary<string, object> AdditionalInformation { get; set; }

        public LogDetail()
        {
            TimeStamp = DateTime.Now;
        }
    }
}