using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czertainly.Auth.Common.Data
{
    public class QueryStringParameters
    {
        const int MaxItemsPerPage = 50;

        private int _itemsPerPage = 10;

        // pagination
        public int PageNumber { get; set; } = 1;
        public int ItemsPerPage
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = (value > MaxItemsPerPage) ? MaxItemsPerPage : value;
            }
        }

        // sorting
        public string SortBy { get; set; }

        public bool SortAscending { get; set; }
    }
}
