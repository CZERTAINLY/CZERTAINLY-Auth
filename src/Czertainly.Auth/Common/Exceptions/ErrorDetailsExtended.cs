using System.Text.Json;

namespace Czertainly.Auth.Common.Exceptions
{
    public class ErrorDetailsExtended : ErrorDetails
    {
        public string Url { get; private set; }
        public string Service { get; private set; }
        public string[] Exception { get; private set; }
        public string[]? InnerException { get; private set; }

        public ErrorDetailsExtended(string url, string service, Exception exception)
            : base(exception)
        {
            Url = url;
            Service = service;

            var exceptionList = new List<string>() { exception.Message };
            if (exception.StackTrace != null) exceptionList.AddRange(exception.StackTrace[6..].Split("\r\n   at "));
            Exception = exceptionList.ToArray();

            if(exception.InnerException != null)
            {
                var innerExceptionList = new List<string>() { exception.InnerException.Message };
                if (exception.InnerException.StackTrace != null) exceptionList.AddRange(exception.InnerException.StackTrace[6..].Split("\r\n   at "));
                InnerException = innerExceptionList.ToArray();
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
