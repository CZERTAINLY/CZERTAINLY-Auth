using System.Net;

namespace Czertainly.Auth.Common.Exceptions
{
    public class EntityNotUniqueException : RequestException
    {
        public EntityNotUniqueException(string message, Exception? innerException = null)
            : base(HttpStatusCode.BadRequest, "ENTITY_NOT_UNIQUE", message, innerException)
        {
        }
    }
}
