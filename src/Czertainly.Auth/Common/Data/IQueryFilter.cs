using System.Runtime.Serialization;

namespace Czertainly.Auth.Common.Data
{
    public interface IQueryFilter
    {
        public string Column { get; set; }
        public string Condition { get; set; }
        public object Value { get; set; }
    }
}
