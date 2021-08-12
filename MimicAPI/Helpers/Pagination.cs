using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class Pagination
    {
        public int pageNumber { get; set; }
        public int pageLimit { get; set; }
        public int totalRegisters { get; set; }
        public int totalPages { get; set; }
    }
}
