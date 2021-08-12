using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class WordUrlQuery
    {
        public DateTime? Date { get; set; }
        public int? PageNumber { get; set; }
        public int? PageLimit { get; set; }
    }
}
