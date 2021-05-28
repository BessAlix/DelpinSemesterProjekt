using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Classes
{
    public class QueryParameters
    {
        const int _maxSize = 100;
        private int _size = 10;
        private int _page = 1;

        public string SortBy { get; set; }
        public string SearchBy { get; set;}

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = Math.Min(_maxSize, value);
            }
        }

        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
            }
        }
    }
}
