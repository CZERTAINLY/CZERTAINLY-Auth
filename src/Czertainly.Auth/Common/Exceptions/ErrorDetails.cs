using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Czertainly.Auth.Common.Exceptions
{
    public class ErrorDetails
    {
        public int StatusCode { get; init; }
        public string Code { get; init; }
        public string Message { get; init; }
        
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
