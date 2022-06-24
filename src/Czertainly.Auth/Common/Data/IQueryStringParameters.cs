using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czertainly.Auth.Common.Data
{
    public interface IQueryStringParameters
    {
        // pagination
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }

        // sorting
        public string Sort { get; set; }

        public Array Filters { get; set; }
    }
}
