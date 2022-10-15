using System.Net;

namespace Czertainly.Auth.Common.Exceptions
{
    public class InvalidActionException : RequestException
    {
        public InvalidActionException(string message, Exception? innerException = null)
            : base(HttpStatusCode.BadRequest, "INVALID_ACTION", message, innerException)
        {
        }
    }
}
