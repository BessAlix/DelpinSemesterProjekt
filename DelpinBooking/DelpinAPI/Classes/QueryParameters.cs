using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinAPI.Classes
{
    public class QueryParameters
    {
        const int _maxSize = 100;
        private int _size = 100;

        public int Page { get; set; }
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
        
       
    }
}
