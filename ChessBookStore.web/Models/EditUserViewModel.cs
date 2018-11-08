using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class EditUserViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public IList<Address> Addresses { get; set; }
    }
}
