using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class SingleUserFromListViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        //public string AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
