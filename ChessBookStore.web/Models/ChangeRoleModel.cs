using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Models
{
    public class ChangeRoleModel
    {
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
