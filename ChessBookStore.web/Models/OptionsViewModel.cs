using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class OptionsViewModel
    {
        public int PriceFrom { get; set; }
        public int PriceTo { get; set; }
        public bool isDiscont { get; set; }
    }
}
