using System.Text.Json;

namespace Czertainly.Auth.Common.Exceptions
{
    public class ErrorDetailsExtended : ErrorDetails
    {
        public string Url { get; init; }
        public string Service { get; init; }
        public string Exception { get; private set; }
        public string? InnerException { get; private set; }
        public string[] StackTrace { get; private set; }

        public ErrorDetailsExtended(Exception exception)
        {
            Exception = exception.Message;
            InnerException = exception.InnerException?.Message;
            //StackTrace = exception.StackTrace.Substring(6, exception.StackTrace.IndexOf("\r\n") - 6);
            StackTrace = exception.StackTrace.Substring(6).Split("\r\n   at ");
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
